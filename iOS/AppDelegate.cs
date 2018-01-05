using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace MessagingSample.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			App.Mp3FilePath = FileAccessHelper.GetLocalFilePath(Constants.FullFileName);
			LoadApplication(new App());


			return base.FinishedLaunching(app, options);
		}
	}
}
