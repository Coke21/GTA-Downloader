using Google.Apis.Download;
using System;
using System.IO;
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

        public static async Task FileAsync(string fileID)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            isExecuted = true;
            await Task.Run(async () =>
            {
                await win.Dispatcher.BeginInvoke((Action)(() => FileData.ButtonsOption("beforeDownload")));
                MemoryStream stream = new MemoryStream();

                var request = FileData.service.Files.Get(fileID);

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
                            using (FileStream file = new FileStream(FileData.getDownloadFolderPath + fileName, FileMode.Create, FileAccess.Write)) { stream.WriteTo(file); stream.Close(); file.Close(); }
                            win.Dispatcher.BeginInvoke((Action)(() => MoveMission(FileData.getDownloadFolderPath + fileName, fileName)));
                            isExecuted = false;
                            break;
                        case DownloadStatus.Failed:
                            win.Dispatcher.BeginInvoke((Action)(() => FileData.ButtonsOption("afterDownload")));
                            break;
                    }
                };
                await request.DownloadAsync(stream);
            });
        }
        private static void MoveMission (string locOfFile, string fileName)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            string destFile = Path.Combine(FileData.folderPath, fileName);
            
            if (File.Exists(destFile)) File.Delete(destFile);

            File.Move(locOfFile, destFile);

            win.TextTopOperationNotice.Text = "";
            win.TextTopOperationProgramNotice.Text = "";

            win.TextTopOperationNotice.Text = $"{fileName} has been moved to your MPMissionsCache folder.";
            win.TextTopOperationNotice.Foreground = Brushes.ForestGreen;

            DispatcherTimer myDispatcherTimer = new DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2, 0);
            myDispatcherTimer.Tick += new EventHandler(HideText);
            myDispatcherTimer.Start();

            void HideText(object sender, EventArgs e)
            {
                win.TextTopOperationNotice.Text = "";
                DispatcherTimer timer = (DispatcherTimer)sender;
                timer.Stop();
            }
            FileData.ButtonsOption("afterDownload");
        }
    }
}
