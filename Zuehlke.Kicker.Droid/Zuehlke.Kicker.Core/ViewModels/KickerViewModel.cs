using System.Threading.Tasks;
using Cirrious.CrossCore.UI;
using Cirrious.MvvmCross.ViewModels;
using Zuehlke.Kicker.Core.Services;

namespace Zuehlke.Kicker.Core.ViewModels
{
    public class KickerViewModel : MvxViewModel
    {
        private readonly IKnowTheKickerState _state;
        private readonly IBackgroundService _backgroundService;

        public KickerViewModel(IKnowTheKickerState state, IBackgroundService backgroundService)
        {
            _state = state;
            _backgroundService = backgroundService;
            Color = MvxColors.Transparent;
            Free = null;

            Reload = new MvxCommand(LoadState);
            StartService = new MvxCommand(_backgroundService.Start);
            StopService = new MvxCommand(_backgroundService.Stop);
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);
            LoadState();
        }

        private void LoadState()
        {
            State = "Loading";
            Color = MvxColors.Transparent;
            Free = null;
            _state.Current().ContinueWith(t =>
            {
                var kicker = t.Result;
                if (kicker != null)
                {
                    Free = kicker.Free;
                    Color = kicker.Free ? MvxColors.Green : MvxColors.Red;
                    State = kicker.Free
                        ? "Frei"
                        : string.Format("Besetzt seit {0}:{1}", kicker.Since.Hour, kicker.Since.Minute);
                }
                else
                {
                    State = "Fehler beim Laden";
                }

                RaiseAllPropertiesChanged();
            },
            TaskScheduler.Current);

            RaiseAllPropertiesChanged();
        }

        public bool? Free { get; private set; }
        public MvxColor Color { get; private set; }
        public string State { get; private set; }
        public MvxCommand Reload { get; private set; }
        public MvxCommand StartService { get; private set; }
        public MvxCommand StopService { get; private set; }
    }
}
