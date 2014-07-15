using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.ViewModels;
using Zuehlke.Kicker.Core.Services;
using Zuehlke.Kicker.Core.ViewModels;

namespace Zuehlke.Kicker.Core
{
    public class App : MvxApplication
    {
        public App()
        {
            Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<KickerViewModel>());
         Mvx.LazyConstructAndRegisterSingleton<IKnowTheKickerState, KickerStateServiceClient>();   
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);
            Cirrious.MvvmCross.Plugins.Color.PluginLoader.Instance.EnsureLoaded();
        }
    }
}
