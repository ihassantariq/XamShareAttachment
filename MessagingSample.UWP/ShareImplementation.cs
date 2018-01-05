using MessagingSample.UWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(ShareImplementation))]
namespace MessagingSample.UWP
{
    /// <summary>
    /// Implementation for Share
    /// </summary>
    public class ShareImplementation : IShare
    {

        string title, text, fileName;
        DataTransferManager dataTransferManager;
        private IStorageFile _downloadedFile;

        /// <summary>
        /// Share a message with compatible services
        /// </summary>
        /// <param name="message">Message to share</param>
        /// <param name="options">Platform specific options</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public Task<bool> Share(ShareMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            try
            {
                title = message.Title;
                text = message.Text;
                fileName = message.Path;
                if (dataTransferManager == null)
                {
                    dataTransferManager = DataTransferManager.GetForCurrentView();
                    dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.ShareTextHandler);
                }
                DataTransferManager.ShowShareUI();

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to share: " + ex.Message);
                return Task.FromResult(false);
            }
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            try
            {
                DataRequest request = e.Request;

                // The Title is mandatory
#if WINDOWS_UWP || WINDOWS_APP
                request.Data.Properties.Title = title ?? Windows.ApplicationModel.Package.Current.DisplayName;
#else
                request.Data.Properties.Title = title ?? string.Empty;
#endif

                if (text != null)
                    request.Data.SetText(text);
                if (fileName != null)
                {
                    _downloadedFile = StorageFile.GetFileFromPathAsync(fileName).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                    List<IStorageFile> files = new List<IStorageFile>();
                    files.Add(_downloadedFile);
                    request.Data.SetStorageItems(files);

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to share: " + ex.Message);
            }
        }
    }

       
}
