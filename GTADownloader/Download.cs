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
        public static bool isExecuted = false;
        public static string downloadSpeed;

        public static async Task FileAsync(string fileID, CancellationToken cancellationToken)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            isExecuted = true;
            await Task.Run(async () =>
            {
                await win.Dispatcher.BeginInvoke((Action)(() => Data.ButtonsOption("beforeDownload")));

                using (MemoryStream stream = new MemoryStream())
                {
                    var request = Data.service.Files.Get(fileID);

                    switch (downloadSpeed)
                    {
                        case "maxSpeed":
                            request.MediaDownloader.ChunkSize = 10000000;
                            break;
                        case "normalSpeed":
                            request.MediaDownloader.ChunkSize = 1000000;
                            break;
                    }
                    request.Fields = "size, name";
                    double checkedValue = Convert.ToDouble(request.Execute().Size);
                    string fileName = request.Execute().Name;

                    request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                double bytesIn = progress.BytesDownloaded;

                                double percentage = bytesIn / checkedValue * 100;

                                double truncated = Math.Truncate(bytesIn / 1000000);
                                double truncated2 = Math.Truncate(checkedValue / 1000000);

                                win.Dispatcher.BeginInvoke((Action)(() =>
                                {
                                    win.textblockDownload.Text = $"Downloading {fileName} - " + truncated + "MB/" + truncated2 + "MB";
                                    win.progressBarDownload.Value = percentage;
                                }));
                                break;
                            case DownloadStatus.Completed:
                                using (FileStream file = new FileStream(Data.getDownloadFolderPath + fileName, FileMode.Create, FileAccess.Write))
                                    stream.WriteTo(file);
                                win.Dispatcher.BeginInvoke((Action)(() => MoveMission(Data.getDownloadFolderPath + fileName, fileName)));
                                isExecuted = false;
                                break;
                            case DownloadStatus.Failed:
                                win.Dispatcher.BeginInvoke((Action)(() => Data.ButtonsOption("afterDownload")));
                                win.Dispatcher.BeginInvoke((Action)(() => WriteNotification($"An error appeared with {fileName} file!", Brushes.Red, 5)));
                                isExecuted = false;
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
        private static void MoveMission (string locOfFile, string fileName)
        {
            string destFile = Path.Combine(Data.folderPath, fileName);
            
            if (File.Exists(destFile)) File.Delete(destFile);

            File.Move(locOfFile, destFile);

            WriteNotification($"{fileName} has been moved to your MPMissionsCache folder", Brushes.ForestGreen);

            Data.ButtonsOption("afterDownload");
        }
        private static void WriteNotification (string text, SolidColorBrush colour, int timeDisplaySec = 3)
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
