using Xamarin.Forms;

namespace MessagingSample
{
	public partial class App : Application
	{
		public static string Mp3FilePath = "";
		public App()
		{
			InitializeComponent();

			MainPage = new MessagingSamplePage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
