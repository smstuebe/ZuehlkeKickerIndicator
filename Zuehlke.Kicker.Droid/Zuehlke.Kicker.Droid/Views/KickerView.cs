using Android.App;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Zuehlke.Kicker.Core.ViewModels;

namespace Zuehlke.Kicker.Droid.Views
{
    [Activity(Label = "Zühlke Kicker Indicator MUC", Icon = "@drawable/icon")]
    [MvxViewFor(typeof(KickerViewModel))]

    public class KickerView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.KickerView);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            var kicker = (KickerViewModel) ViewModel;
            kicker.PropertyChanged += kicker_PropertyChanged;
        }

        void kicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var reload = FindViewById<ImageButton>(Resource.Id.kicker_state_reload);
            var kicker = (KickerViewModel)ViewModel;
            if (kicker.Free.HasValue)
            {
                if (kicker.Free.Value)
                {
                    reload.SetImageResource(Resource.Drawable.IconFree);
                }
                else
                {
                    reload.SetImageResource(Resource.Drawable.IconNotFree);
                }
            }
            else
            {
                reload.SetImageResource(Resource.Drawable.IconLoading);
            }
        }
    }
}