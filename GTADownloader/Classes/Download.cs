using Google.Apis.Download;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GTADownloader.Classes;

namespace GTADownloader
{
    class Download
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;
        public static async Task FileAsync(string fileId, ListViewProperties.ListViewItems selectedItem, CancellationToken cancellationToken, string option = "missionFile")
        {
            Win.StopDownloadButton.Visibility = Visibility.Visible;

            var request = Data.GetFileRequest(fileId, "size, name");
            request.MediaDownloader.ChunkSize = 10000000;

            var requestedFile = await request.ExecuteAsync();

            string mFPath = Path.Combine(DataProperties.GetArma3MissionFolderPath, requestedFile.Name);
            string programPath = Path.Combine(DataProperties.GetProgramFolderPath, requestedFile.Name + ".exe");

            using (MemoryStream stream = new MemoryStream())
            using (FileStream file = new FileStream(option == "missionFile" ? mFPath : programPath, FileMode.Create, FileAccess.Write))
            {
                request.MediaDownloader.ProgressChanged += progress =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            double bytesIn = progress.BytesDownloaded;

                            double currentValue = Math.Truncate(bytesIn / 1000000);
                            double totalValue = Math.Truncate((double)requestedFile.Size / 1000000);

                            Win.Dispatcher.Invoke(() => Win.TextBlockDownload.Text = $"Downloading '{requestedFile.Name}' - " + currentValue + "MB/" + totalValue + "MB");
                            break;

                        case DownloadStatus.Failed:
                            if (selectedItem != null)
                            {
                                selectedItem.IsMissionUpdated = "Outdated";
                                selectedItem.IsModifiedTimeUpdated = "Outdated";
                            }
                            Win.Dispatcher.Invoke(() => Win.TextBlockDownload.Text = string.Empty);
                            Win.Dispatcher.Invoke(() => Win.StopDownloadButton.Visibility = Visibility.Hidden);
                            break;

                        case DownloadStatus.Completed:
                            stream.WriteTo(file);
                            if (selectedItem != null)
                            {
                                selectedItem.IsMissionUpdated = "Updated";
                                selectedItem.IsModifiedTimeUpdated = "Updated";
                            }
                            Win.Dispatcher.Invoke(() => Win.TextBlockDownload.Text = string.Empty);
                            Win.Dispatcher.Invoke(() => Win.StopDownloadButton.Visibility = Visibility.Hidden);
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

                File.Move(DataProperties.GetProgramFolderPath + DataProperties.GetProgramName, DataProperties.GetProgramFolderPath + DataProperties.GetProgramName + "OLD" + ".exe");

                Process.Start(new ProcessStartInfo()
                {
                    Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + DataProperties.GetProgramFolderPath + DataProperties.GetProgramName + "OLD" + ".exe",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                });
                Win.Window_Closing(null, new CancelEventArgs());
            }
        }
    }
}
