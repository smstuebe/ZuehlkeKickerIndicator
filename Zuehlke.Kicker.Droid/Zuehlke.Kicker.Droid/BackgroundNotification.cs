using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Cirrious.CrossCore;
using Zuehlke.Kicker.Core.Services;
using Zuehlke.Kicker.Droid.Views;

namespace Zuehlke.Kicker.Droid
{
    public class BackgroundNotificationService : IBackgroundService
    {
        private readonly Context _context;

        public BackgroundNotificationService(Context context)
        {
            _context = context;
        }

        public void Start()
        {
            BackgroundNotification.KickerState = Mvx.Resolve<IKnowTheKickerState>();
            _context.StartService(new Intent(_context, typeof(BackgroundNotification)));
        }

        public void Stop()
        {
            _context.StopService(new Intent(_context, typeof(BackgroundNotification)));
        }
    }



    [Service]
    public class BackgroundNotification : Service
    {
        internal static IKnowTheKickerState KickerState
        {
            get;
            set;
        }

        private KickerState _lastState;
        private IKnowTheKickerState _kickerState;
        private Timer _timer;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);
            return StartCommandResult.Sticky;
        }

        public override void OnStart(Intent intent, int startId)
        {
            base.OnStart(intent, startId);
            _kickerState = KickerState ?? new KickerStateServiceClient();
            _timer = new Timer(OnUpdateState, null, 0, 30000);
            //Notify("Getting started" + DateTime.Now, "Service is started", Resource.Drawable.Icon);
            //Vibrate();
        }

        private void OnUpdateState(object o)
        {
            try
            {
                var stateTask = _kickerState.Current();
                stateTask.Wait(TimeSpan.FromSeconds(15));
                if (stateTask.IsCompleted)
                {
                    var state = stateTask.Result;
                    if (state != null && (_lastState == null || _lastState.Free != state.Free))
                    {
                        Notify(state);
                        Vibrate();
                    }
                    else
                    {
                        //Notify("State unchanged " + DateTime.Now, "unchanged", Resource.Drawable.Icon);
                    }
                    _lastState = state ?? _lastState;
                }
                else if (stateTask.IsCanceled)
                {
                    //Notify("Update cancelled" + DateTime.Now, "cancelled", Resource.Drawable.Icon);
                    //Vibrate();
                }
                else if (stateTask.IsFaulted)
                {
                    //Notify("ERROR in task:", stateTask.Exception.Message, Resource.Drawable.Icon);
                    //Vibrate();
                }
            }
            catch(Exception ex)
            {
                try
                {
                    //Notify("ERROR:", ex.Message, Resource.Drawable.Icon);
                    //Vibrate();
                }
                catch
                {
                }
            }
        }

        public override void OnDestroy()
        {
            _timer.Dispose();
            //Notify("Getting killed" + DateTime.Now, "Service is killed", Resource.Drawable.Icon);
            //Vibrate();
            base.OnDestroy();
        }

        private void Vibrate()
        {
            var vibrator = (Vibrator)GetSystemService(VibratorService);
            vibrator.Vibrate(500);
        }

        private void Notify(KickerState state)
        {
            Notify("Kicker " + (state.Free ? "frei" : "besetzt"),
                "Kicker " + (state.Free ? "frei seit " + state.Since : "besetzt seit " + state.Since),
                state.Free ? Resource.Drawable.IconFree : Resource.Drawable.IconNotFree);
        }

        private void Notify(string contentTitle, string contentDetail, int icon)
        {
            var nMgr = (NotificationManager) GetSystemService(NotificationService);
            var notification = new Notification(icon, contentTitle);
            var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof (SplashScreen)), 0);

            notification.SetLatestEventInfo(this,
                contentTitle,
                contentDetail,
                pendingIntent);
            nMgr.Notify(0, notification);
        }
    }
}