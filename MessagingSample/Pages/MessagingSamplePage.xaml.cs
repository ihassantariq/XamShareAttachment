using MessagingSample.ViewModels;
using Xamarin.Forms;

namespace MessagingSample
{
	public partial class MessagingSamplePage : ContentPage
	{
		public MessageSamplePageViewModel viewModel;
		public MessagingSamplePage()
		{
			InitializeComponent();
			viewModel = (MessageSamplePageViewModel)BindingContext;
			viewModel.ContentPage = this;
		}
	}
}
