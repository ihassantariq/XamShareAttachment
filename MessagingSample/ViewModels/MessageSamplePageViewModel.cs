using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Plugin.Messaging;
using Plugin.ShareFile;
using Xamarin.Forms;

namespace MessagingSample.ViewModels
{
	enum MessagingOptions
	{
		Cancel = 0,
		Email = 1,
		Message = 2,
		Share = 3,
	};
	public class MessageSamplePageViewModel : BaseViewModel
	{

		public ContentPage ContentPage { set; get; }
		#region Commands
		public ICommand OpenSheetCommand => new Command(OpenSheetOptions);
		#endregion Commands

		public MessageSamplePageViewModel()
		{ }
		private async void OpenSheetOptions()
		{
			var action = await ContentPage?.DisplayActionSheet("Select Sending Options?", MessagingOptions.Cancel.ToString(), null, MessagingOptions.Email.ToString(), MessagingOptions.Message.ToString(), MessagingOptions.Share.ToString());
			if (action == MessagingOptions.Email.ToString())
			{
				SenEmail();
			}
			else if (action == MessagingOptions.Message.ToString())
			{
				SendMessage();
			}
			else if (action == MessagingOptions.Share.ToString())
			{
				//CrossShareFile.Current.ShareLocalFile(App.Mp3FilePath, "Test Path");

				//if (!CrossShare.IsSupported)
				//return;

				await DependencyService.Get<IShare>().Share(new ShareMessage
				{
					Title = Constants.EmailSubject,
					Text = Constants.EmailBody,
					Path = App.Mp3FilePath,
				});
				//await CrossShare.Current.Share(new ShareMessage
				//{
				//	Title = Constants.EmailSubject,
				//	Text = Constants.EmailBody,
				//	Url = Constants.ShareUrl
				//});
			}
		}
		void SendMessage()
		{
			var smsMessenger = CrossMessaging.Current.SmsMessenger;
			if (smsMessenger.CanSendSms)
				smsMessenger.SendSms(Constants.SMSNumber, Constants.SMSText);
		}
		void SenEmail()
		{
			var emailMessenger = CrossMessaging.Current.EmailMessenger;
			if (emailMessenger.CanSendEmailAttachments)
			{

				String path = App.Mp3FilePath;
				//Construct email with attachment.
				var email = new EmailMessageBuilder()
				.To(Constants.EmailTo)
				.Subject(Constants.EmailSubject)
				.Body(Constants.EmailBody)
				.WithAttachment(path, Constants.FileExtension).Build();
				emailMessenger.SendEmail(email);


			}
		}
	}

}

