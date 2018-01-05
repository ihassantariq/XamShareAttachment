using System;
using System.IO;
using Foundation;

namespace MessagingSample.iOS
{
	public class FileAccessHelper
	{
		public static string GetLocalFilePath(string filename)
		{
			string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string libFolder = Path.Combine(docFolder, "..", "Library", "EmbededResource");

			if (!Directory.Exists(libFolder))
			{
				Directory.CreateDirectory(libFolder);
			}

			string mp3Path = Path.Combine(libFolder, filename);

			CopyDatabaseIfNotExists(mp3Path);

			return mp3Path;
		}

		private static void CopyDatabaseIfNotExists(string mp3Path)
		{
			if (!File.Exists(mp3Path))
			{
				var existingMp3 = NSBundle.MainBundle.PathForResource(Constants.FileName, Constants.FileExtension);
				File.Copy(existingMp3, mp3Path);
			}
		}
	}
}
