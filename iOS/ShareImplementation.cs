using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MessagingSample.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ShareImplementation))]
namespace MessagingSample.iOS
{
	/// <summary>
	/// Implementation for Share
	/// </summary>
	public class ShareImplementation : IShare
	{



		/// <summary>
		/// Share a message with compatible services
		/// </summary>
		/// <param name="message">Message to share</param>
		/// <param name="options">Platform specific options</param>
		/// <param name="excludedActivityTypes">UIActivityTypes that should not be displayed</param>
		/// <returns>True if the operation was successful, false otherwise</returns>
		public async Task<bool> Share(ShareMessage message)
		{
			try
			{
				if (message != null && String.IsNullOrEmpty(message.Title) && String.IsNullOrEmpty(message.Text) && String.IsNullOrEmpty(message.Path))
				{
					Console.WriteLine("Plugin.ShareFile: ShareLocalFile Warning: localFilePath null or empty");
					return false;
				}

				var rootController = GetVisibleViewController();
				var sharedItems = new System.Collections.Generic.List<NSObject>();
				var fileName = Path.GetFileName(message.Path);



				// file url
				var fileUrl = NSUrl.FromFilename(message.Path);
				sharedItems.Add(fileUrl);

				//// Text
				string body = message.Title + System.Environment.NewLine + message.Text;
				sharedItems.Add(new NSString(body));

				UIActivity[] applicationActivities = null;
				var activityViewController = new UIActivityViewController(sharedItems.ToArray(), applicationActivities);

				// Subject
				if (!string.IsNullOrWhiteSpace(message.Title))
					activityViewController.SetValueForKey(NSObject.FromObject(message.Title), new NSString("subject"));

				await rootController.PresentViewControllerAsync(activityViewController, true);
				return true;
			}
			catch (Exception ex)
			{
				if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
					Console.WriteLine("Plugin.ShareFile: ShareLocalFile Exception: {0}", ex);
				return false;
			}
		}

		/// <summary>
		/// Gets the visible view controller.
		/// </summary>
		/// <returns>The visible view controller.</returns>
		UIViewController GetVisibleViewController()
		{
			UIViewController viewController = null;
			var window = UIApplication.SharedApplication.KeyWindow;


			if (window != null && window.WindowLevel == UIWindowLevel.Normal)
				viewController = window.RootViewController;

			if (viewController == null)
			{
				window = UIApplication.SharedApplication.Windows.OrderByDescending(w => w.WindowLevel).FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);
				if (window == null)
					throw new InvalidOperationException("Could not find current view controller");
				else
					viewController = window.RootViewController;
			}

			while (viewController.PresentedViewController != null)
				viewController = viewController.PresentedViewController;


			return viewController;
		}

		/// <summary>
		/// Converts the <see cref="ShareUIActivityType"/> to its native representation.
		/// Returns null if the activity type is invalid or not supported on the current platform.
		/// </summary>
		/// <param name="type">The activity type</param>
		/// <returns>The native representation of the activity type or null</returns>
		NSString GetUIActivityType(ShareUIActivityType type)
		{
			switch (type)
			{
				case ShareUIActivityType.AssignToContact:
					return UIActivityType.AssignToContact;
				case ShareUIActivityType.CopyToPasteboard:
					return UIActivityType.CopyToPasteboard;
				case ShareUIActivityType.Mail:
					return UIActivityType.Mail;
				case ShareUIActivityType.Message:
					return UIActivityType.Message;
				case ShareUIActivityType.PostToFacebook:
					return UIActivityType.PostToFacebook;
				case ShareUIActivityType.PostToTwitter:
					return UIActivityType.PostToTwitter;
				case ShareUIActivityType.PostToWeibo:
					return UIActivityType.PostToWeibo;
				case ShareUIActivityType.Print:
					return UIActivityType.Print;
				case ShareUIActivityType.SaveToCameraRoll:
					return UIActivityType.SaveToCameraRoll;

				case ShareUIActivityType.AddToReadingList:
					return UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ? UIActivityType.AddToReadingList : null;
				case ShareUIActivityType.AirDrop:
					return UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ? UIActivityType.AirDrop : null;
				case ShareUIActivityType.PostToFlickr:
					return UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ? UIActivityType.PostToFlickr : null;
				case ShareUIActivityType.PostToTencentWeibo:
					return UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ? UIActivityType.PostToTencentWeibo : null;
				case ShareUIActivityType.PostToVimeo:
					return UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ? UIActivityType.PostToVimeo : null;

				case ShareUIActivityType.OpenInIBooks:
					return UIDevice.CurrentDevice.CheckSystemVersion(9, 0) ? UIActivityType.OpenInIBooks : null;

				default:
					return null;
			}
		}
	}
}
