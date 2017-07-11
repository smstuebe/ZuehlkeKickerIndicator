using Xamarin.Forms;

namespace Zuehlke.KickerIndicator
{
	public partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();
		    this.BindingContext = new MainPageViewModel();
		}
	}
}
