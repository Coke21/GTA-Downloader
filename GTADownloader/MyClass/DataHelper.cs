using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Threading.Tasks;
using System.Windows;

namespace GTADownloader
{
    class DataHelper
    {
        public static DriveService service = new DriveService(new BaseClientService.Initializer()
        {
            ApiKey = "AIzaSyB8KixGHl2SPwQ5HJixKGm7IGbOYbpuc1w"
        });
        public static void EnableTaskBar()
        {
            Data.notifyIcon.Visible = true;
            Data.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            Data.notifyIcon.Text = "GTA Mission Downloader";
            Data.notifyIcon.Click += (sender, e) => NotifyIconBalloonTipClicked();
        }
        public static async void NotifyIconBalloonTipClicked(bool stopOnStart = true, bool updateFiles = true)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            if (stopOnStart) win.StopOnStart();
            win.WindowState = WindowState.Normal;
            win.Show();
            if (updateFiles) await CheckForUpdate.UpdateAsync(Data.ctsOnStart.Token);
        }
        public static FilesResource.GetRequest GetFileRequest(string fileID, string field)
        {
            var request = service.Files.Get(fileID);
            request.Fields = $"{field}";

            return request;
        }
        public static async Task PopulateFileNameArray()
        {
            int i = 0;
            foreach (var item in Data.fileIDArray)
            {
                var request = await GetFileRequest(item, "name").ExecuteAsync();
                Data.fileNameArray[i++] = request.Name;
            }
        }
        public static void ButtonsOption(string whichOption)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            switch (whichOption)
            {
                case "beforeDownload":
                    win.progressBarDownload.Visibility = Visibility.Visible;
                    win.textblockDownload.Visibility = Visibility.Visible;

                    win.S1AltisButton.IsEnabled = false;
                    win.S2AltisButton.IsEnabled = false;

                    win.S3MaldenButton.IsEnabled = false;
                    win.S3TanoaButton.IsEnabled = false;

                    win.S1S2AltisButton.IsEnabled = false;
                    win.S3MaldenS3TanoaButton.IsEnabled = false;
                    win.AllFiles.IsEnabled = false;

                    win.StopDownloadName.Visibility = Visibility.Visible;
                    break;
                case "afterDownload":
                    win.progressBarDownload.Visibility = Visibility.Hidden;
                    win.textblockDownload.Visibility = Visibility.Hidden;

                    win.S1AltisButton.IsEnabled = true;
                    win.S2AltisButton.IsEnabled = true;

                    win.S3MaldenButton.IsEnabled = true;
                    win.S3TanoaButton.IsEnabled = true;

                    win.S1S2AltisButton.IsEnabled = true;
                    win.S3MaldenS3TanoaButton.IsEnabled = true;
                    win.AllFiles.IsEnabled = true;

                    win.textblockDownload.Text = "";
                    win.progressBarDownload.Value = 0;
                    win.StopDownloadName.Visibility = Visibility.Hidden;
                    break;
                case "optionsCheckBoxOff":
                    win.S1AltisCheckBox.IsChecked = false;
                    win.S2AltisCheckBox.IsChecked = false;
                    win.S3MaldenCheckBox.IsChecked = false;
                    win.S3TanoaCheckBox.IsChecked = false;
                    break;
                case "deleteChangesToRegistry":
                    win.StartUpCheckBox.IsChecked = false;
                    win.HiddenCheckBox.IsChecked = false;
                    win.RunTSAutoCheckBox.IsChecked = false;
                    win.NotificationCheckBox.IsChecked = false;
                    win.AutomaticUpdateCheckBox.IsChecked = false;
                    break;
            }
        }
    }
}
