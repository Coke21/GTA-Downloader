using Google.Apis.Download;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace GTADownloader
{
    class Download
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;
        public static async Task FileAsync(string fileID, CancellationToken cancellationToken, string option = "missionFile")
        {
            DataHelper.ButtonsOption("beforeDownload");

            var request = DataHelper.GetFileRequest(fileID, "size, name");
            var requestedFile = await request.ExecuteAsync();

            string mFPath = Path.Combine(DataProperties.GetArma3MissionFolderPath, requestedFile.Name);
            string programPath = Path.Combine(DataProperties.GetProgramFolderPath, requestedFile.Name + ".exe");

            using (MemoryStream stream = new MemoryStream())
            using (FileStream file = new FileStream(option == "missionFile" ? mFPath : programPath, FileMode.Create, FileAccess.Write))
            {
                switch (DataProperties.DownloadSpeed)
                {
                    case "maxSpeed":
                        request.MediaDownloader.ChunkSize = 10000000;
                        break;
                    case "normalSpeed":
                        request.MediaDownloader.ChunkSize = 1000000;
                        break;
                }

                request.MediaDownloader.ProgressChanged += async (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            double bytesIn = progress.BytesDownloaded;
                            var checkedValue = await request.ExecuteAsync();

                            double percentage = bytesIn / (double)checkedValue.Size * 100;

                            double truncated = Math.Truncate(bytesIn / 1000000);
                            double truncated2 = Math.Truncate((double)checkedValue.Size / 1000000);

                            Win.Dispatcher.Invoke(() => Win.textblockDownload.Text = $"Downloading {requestedFile.Name} - " + truncated + "MB/" + truncated2 + "MB");
                            Win.Dispatcher.Invoke(() => Win.progressBarDownload.Value = percentage);
                            break;
                        case DownloadStatus.Completed:
                            stream.WriteTo(file);
                            Win.Dispatcher.Invoke(() => DataHelper.ButtonsOption("afterDownload"));
                            Win.Dispatcher.Invoke(() => Notify(option == "missionFile" ? $"{requestedFile.Name} has been downloaded!" : "The updated version of GTA program has been downloaded!", Brushes.ForestGreen));
                            break;
                        case DownloadStatus.Failed:
                            Win.Dispatcher.Invoke(() => DataHelper.ButtonsOption("afterDownload"));
                            Win.Dispatcher.Invoke(() => Notify($"An error appeared with {requestedFile.Name} file!", Brushes.Red, 5));
                            Data.missionFileListID.Remove(fileID);
                            break;
                    }
                };
                try
                {
                    await request.DownloadAsync(stream, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
            if (option == "programUpdate")
            {
                Process.Start(programPath);
                Process.Start(new ProcessStartInfo()
                {
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + DataProperties.GetProgramFolderPath + DataProperties.GetProgramName + "\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                });
                Win.Close();
            }
        }
        private static void Notify (string text, SolidColorBrush colour, int timeDisplaySec = 3)
        {
            Win.TextTopOperationNotice.Text = "";
            Win.TextTopOperationProgramNotice.Text = "";

            Win.TextTopOperationNotice.Text = text;
            Win.TextTopOperationNotice.Foreground = colour;

            DispatcherTimer myDispatcherTimer = new DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, timeDisplaySec, 0);
            myDispatcherTimer.Tick += new EventHandler(HideNotificationText);
            myDispatcherTimer.Start();

            void HideNotificationText(object sender, EventArgs e)
            {
                Win.TextTopOperationNotice.Text = "";
                DispatcherTimer timer = (DispatcherTimer)sender;
                timer.Stop();
            }
        }
    }
}
