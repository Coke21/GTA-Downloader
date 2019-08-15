using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Threading.Tasks;
using System.Windows;

namespace GTADownloader
{
    class DataHelper
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;

        public static DriveService Service = new DriveService(new BaseClientService.Initializer()
        {
            ApiKey = "AIzaSyB8KixGHl2SPwQ5HJixKGm7IGbOYbpuc1w"
        });
        public static FilesResource.GetRequest GetFileRequest(string fileId, string field)
        {
            var request = Service.Files.Get(fileId);
            request.Fields = $"{field}";

            return request;
        }
        public static async Task PopulateFileNameArray()
        {
            int i = 0;
            foreach (var item in Data.FileIdArray)
            {
                var request = await GetFileRequest(item, "name").ExecuteAsync();
                Data.FileNameArray[i++] = request.Name;
            }
        }
        public static void ButtonsOption(string whichOption)
        {
            switch (whichOption)
            {
                case "beforeDownload":
                    Win.ProgressBarDownload.Visibility = Visibility.Visible;
                    Win.TextBlockDownload.Visibility = Visibility.Visible;

                    Win.S1AltisButton.IsEnabled = false;
                    Win.S2AltisButton.IsEnabled = false;

                    Win.S3TanoaButton.IsEnabled = false;
                    Win.S3EnochButton.IsEnabled = false;

                    Win.S1S2AltisButton.IsEnabled = false;
                    Win.S3MaldenS2LivoniaButton.IsEnabled = false;
                    Win.AllFilesButton.IsEnabled = false;

                    Win.StopDownloadName.Visibility = Visibility.Visible;
                    break;
                case "afterDownload":
                    Win.ProgressBarDownload.Visibility = Visibility.Hidden;
                    Win.TextBlockDownload.Visibility = Visibility.Hidden;

                    Win.S1AltisButton.IsEnabled = true;
                    Win.S2AltisButton.IsEnabled = true;

                    Win.S3TanoaButton.IsEnabled = true;
                    Win.S3EnochButton.IsEnabled = true;

                    Win.S1S2AltisButton.IsEnabled = true;
                    Win.S3MaldenS2LivoniaButton.IsEnabled = true;
                    Win.AllFilesButton.IsEnabled = true;

                    Win.StopDownloadName.Visibility = Visibility.Hidden;

                    Win.TextBlockDownload.Text = "";
                    Win.ProgressBarDownload.Value = 0;
                    break;
                case "optionsCheckBoxOff":
                    Win.S1AltisCheckBox.IsChecked = false;
                    Win.S2AltisCheckBox.IsChecked = false;
                    Win.S3TanoaCheckBox.IsChecked = false;
                    Win.S3EnochCheckBox.IsChecked = false;
                    break;
            }
        }
    }
}
