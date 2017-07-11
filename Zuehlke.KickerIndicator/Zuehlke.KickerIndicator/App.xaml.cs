using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace Zuehlke.KickerIndicator
{
	public partial class App : Application
	{
		public App ()
		{
		    CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture = new CultureInfo("de-DE");


            InitializeComponent();
            
			MainPage = new Zuehlke.KickerIndicator.MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
