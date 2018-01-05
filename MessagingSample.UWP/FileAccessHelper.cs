using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MessagingSample.UWP
{
    public class FileAccessHelper
    {
        public async static Task<string> GetLocalFilePath(string filename)
        {
            IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;

            var existingFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(filename);

            bool isExist = await CheckForExistingFile(filename);
            if (!isExist)
            {
                await existingFile.CopyAsync(applicationFolder);
            }

            return existingFile.Path;
        }

        private async static Task<bool>  CheckForExistingFile(string filePath)
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(Uri.EscapeDataString(filePath));
                //no exception means file exists
                return true;
            }
            catch (FileNotFoundException ex)
            {
                //find out through exception
                return false;
            }
        }

    }
}
