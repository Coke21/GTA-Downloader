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
        public static string DownloadSpeed { get; set; }
        public static async Task FileAsync(string fileID, CancellationToken cancellationToken, string option = "missionFile")
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            await Task.Run(async() =>
            {
                win.Dispatcher.Invoke(() => Data.ButtonsOption("beforeDownload"));

                var request = Data.GetFileRequest(fileID, "size, name");
                string fileName = request.Execute().Name;

                string mFPath = Path.Combine(Data.getArma3MissionFolderPath, fileName);
                string programPath = Path.Combine(Data.getProgramFolderPath, fileName + ".exe");

                using (MemoryStream stream = new MemoryStream())
                using (FileStream file = new FileStream(option == "missionFile" ? mFPath : programPath, FileMode.Create, FileAccess.Write))
                {
                    switch (DownloadSpeed)
                    {
                        case "maxSpeed":
                            request.MediaDownloader.ChunkSize = 10000000;
                            break;
                        case "normalSpeed":
                            request.MediaDownloader.ChunkSize = 1000000;
                            break;
                    }

                    request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                double bytesIn = progress.BytesDownloaded;
                                double checkedValue = (double)(request.Execute().Size);

                                double percentage = bytesIn / checkedValue * 100;

                                double truncated = Math.Truncate(bytesIn / 1000000);
                                double truncated2 = Math.Truncate(checkedValue / 1000000);

                                win.Dispatcher.Invoke(() => win.textblockDownload.Text = $"Downloading {fileName} - " + truncated + "MB/" + truncated2 + "MB");
                                win.Dispatcher.Invoke(() => win.progressBarDownload.Value = percentage);
                                break;
                            case DownloadStatus.Completed:
                                stream.WriteTo(file);
                                win.Dispatcher.Invoke(() => Data.ButtonsOption("afterDownload"));
                                win.Dispatcher.Invoke(() => Notify(option == "missionFile" ? $"{fileName} has been moved to your MPMissionsCache folder" : "The updated version of GTA program has been downloaded!", Brushes.ForestGreen));
                                break;
                            case DownloadStatus.Failed:
                                win.Dispatcher.Invoke(() => Data.ButtonsOption("afterDownload"));
                                win.Dispatcher.Invoke(() => Notify($"An error appeared with {fileName} file!", Brushes.Red, 5));
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
                        Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + Data.getProgramFolderPath + Data.getProgramName + "\"",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        FileName = "cmd.exe"
                    });

                    win.Dispatcher.Invoke(() => win.Close());
                }
            });
        }
        private static void Notify (string text, SolidColorBrush colour, int timeDisplaySec = 3)
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
