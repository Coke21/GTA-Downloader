using Google.Apis.Download;
using System;
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
        public static string downloadSpeed;
        public static async Task FileAsync(string fileID, CancellationToken cancellationToken)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            await Task.Run(async() =>
            {
                win.Dispatcher.Invoke(() => Data.ButtonsOption("beforeDownload"));

                var request = Data.service.Files.Get(fileID);
                request.Fields = "size, name";
                string fileName = request.Execute().Name;

                using (MemoryStream stream = new MemoryStream())
                using (FileStream file = new FileStream(Data.getMissionFolderPath + fileName, FileMode.Create, FileAccess.Write))
                {

                    switch (downloadSpeed)
                    {
                        case "maxSpeed":
                            request.MediaDownloader.ChunkSize = 10000000;
                            break;
                        case "normalSpeed":
                            request.MediaDownloader.ChunkSize = 1000000;
                            break;
                    }
                    double checkedValue = Convert.ToDouble(request.Execute().Size);

                    request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                double bytesIn = progress.BytesDownloaded;

                                double percentage = bytesIn / checkedValue * 100;

                                double truncated = Math.Truncate(bytesIn / 1000000);
                                double truncated2 = Math.Truncate(checkedValue / 1000000);

                                win.Dispatcher.Invoke(() =>
                                {
                                    win.textblockDownload.Text = $"Downloading {fileName} - " + truncated + "MB/" + truncated2 + "MB";
                                    win.progressBarDownload.Value = percentage;
                                });
                                break;
                            case DownloadStatus.Completed:
                                stream.WriteTo(file);
                                win.Dispatcher.Invoke(() => Data.ButtonsOption("afterDownload"));
                                win.Dispatcher.Invoke(() => TypeNotification($"{fileName} has been moved to your MPMissionsCache folder", Brushes.ForestGreen));
                                break;
                            case DownloadStatus.Failed:
                                stream.WriteTo(file);
                                win.Dispatcher.Invoke(() => Data.ButtonsOption("afterDownload"));
                                win.Dispatcher.Invoke(() => TypeNotification($"An error appeared with {fileName} file!", Brushes.Red, 5));
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
                    }
                }
            });
        }
        private static void TypeNotification (string text, SolidColorBrush colour, int timeDisplaySec = 3)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;

            win.TextTopOperationNotice.Text = "";
            win.TextTopOperationProgramNotice.Text = "";

            win.TextTopOperationNotice.Text = text;
            win.TextTopOperationNotice.Foreground = colour;

            DispatcherTimer myDispatcherTimer = new DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, timeDisplaySec, 0);
            myDispatcherTimer.Tick += new EventHandler(HideNotificationText);
            myDispatcherTimer.Start();

            void HideNotificationText(object sender, EventArgs e)
            {
                win.TextTopOperationNotice.Text = "";
                DispatcherTimer timer = (DispatcherTimer)sender;
                timer.Stop();
            }
        }
    }
}
