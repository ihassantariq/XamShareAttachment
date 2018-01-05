using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using MessagingSample.Droid;
using static Android.Graphics.BitmapFactory;

[assembly: Xamarin.Forms.Dependency(typeof(ShareImplementation))]
namespace MessagingSample.Droid
{
	/// <summary>
	/// Implementation for Feature
	/// </summary>
	public class ShareImplementation : IShare
	{


		/// <summary>
		/// Share a message with compatible services
		/// </summary>
		/// <param name="message">Message to share</param>
		/// <param name="options">Platform specific options</param>
		/// <returns>True if the operation was successful, false otherwise</returns>
		public Task<bool> Share(ShareMessage message)
		{
			if (message == null && String.IsNullOrEmpty(message.Text) && String.IsNullOrEmpty(message.Title) && String.IsNullOrEmpty(message.Path))
				throw new ArgumentNullException(nameof(message));

			try
			{
				var items = new List<string>();
				if (message.Text != null)
					items.Add(message.Text);

				if (!message.Path.StartsWith("file://"))
					message.Path = string.Format("file://{0}", message.Path);

				var fileUri = Android.Net.Uri.Parse(message.Path);

				var intent = new Intent(Intent.ActionSend);
				intent.SetType("*/*");
				if (message.Title != null)
					intent.PutExtra(Intent.ExtraSubject, message.Title);
				intent.PutExtra(Intent.ExtraText, string.Join(Environment.NewLine, items));
				//intent.PutExtra(Intent.ExtraText, string.Join(Environment.NewLine, items));
				intent.PutExtra(Intent.ExtraStream, fileUri);
				intent.AddFlags(ActivityFlags.GrantReadUriPermission);


				var chooserIntent = Intent.CreateChooser(intent, message?.Title);
				chooserIntent.SetFlags(ActivityFlags.ClearTop);
				chooserIntent.SetFlags(ActivityFlags.NewTask);
				Android.App.Application.Context.StartActivity(chooserIntent);

				return Task.FromResult(true);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Unable to share: " + ex.Message);
				return Task.FromResult(false);
			}
		}
	}
}
