using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace GTADownloader
{
    class CheckForUpdate
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;
        public static async Task UpdateAsync(CancellationToken cancellationToken)
        {
            foreach (var file in Data.FileIdArray.Zip(Data.FileNameArray, Tuple.Create))
            {
                var requestedFile = await DataHelper.GetFileRequest(file.Item1, "size").ExecuteAsync();
                string fileName = file.Item2;

                string fileLoc = Path.Combine(DataProperties.GetArma3MissionFolderPath, fileName);
                long fileSizeOnComputer = 0;

                if (cancellationToken.IsCancellationRequested) return;
                try
                {
                    fileSizeOnComputer = new FileInfo(fileLoc).Length;
                }
                catch (FileNotFoundException)
                {
                    UpdateNotiMf(fileName, "missing", Brushes.Black);
                }
                if (File.Exists(fileLoc))
                    if (requestedFile.Size == fileSizeOnComputer)
                        UpdateNotiMf(fileName, "updated", Brushes.ForestGreen);
                    else
                        UpdateNotiMf(fileName, "outdated", Brushes.Red);
            }

            var requestedProgram = await DataHelper.GetFileRequest(Data.ProgramId, "size").ExecuteAsync();
            long gtaSizeProgramOnComputer = new FileInfo(DataProperties.GetProgramFolderPath + DataProperties.GetProgramName).Length;

            if (requestedProgram.Size == gtaSizeProgramOnComputer)
                UpdateNotiProgram("updated", Brushes.ForestGreen);
            else
            {
                UpdateNotiProgram("outdated", Brushes.Red);
                Win.ProgramUpdateName.Visibility = Visibility.Visible;
                Win.ReadChangelogName.Visibility = Visibility.Visible;

                if (Win.GtaUpdateCheckBox.IsChecked.Value)
                {
                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("A new update for GTA program has been detected. Download it?", "Update", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                        await Download.FileAsync(Data.ProgramId, Data.CtsStopDownloading.Token, "programUpdate");
                }
            }
        }
        private static void UpdateNotiMf(string fileName, string status, SolidColorBrush colour) => Win.TextTopOperationNotice.Inlines.Add(new Run($"{fileName} is {status}!\n") { Foreground = colour });
        private static void UpdateNotiProgram(string status, SolidColorBrush colour) => Win.TextTopOperationProgramNotice.Inlines.Add(new Run($"The GTA program is {status}!") { Foreground = colour });

        public static async Task TypeOfNotificationAsync(string whichOption, CancellationToken cancellationToken)
        {
            Data.NotifyIcon.BalloonTipClicked += (sender, e) => Notification.NotifyIconBalloonTipClicked();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (Data.MissionFileListId.Count > 0)
                {
                    foreach (var fileId in Data.MissionFileListId.ToList())
                    {
                        var requestedFile = await DataHelper.GetFileRequest(fileId, "size, name").ExecuteAsync();

                        string fileLoc = Path.Combine(DataProperties.GetArma3MissionFolderPath, requestedFile.Name);

                        long fileSizeOnComputer = 0;
                        try
                        {
                            fileSizeOnComputer = new FileInfo(fileLoc).Length;
                        }
                        catch (FileNotFoundException)
                        {
                        }
                        if (fileSizeOnComputer != requestedFile.Size)
                        {
                            switch (whichOption)
                            {
                                case "notification":
                                    Data.NotifyIcon.ShowBalloonTip(4000, "Download now!", $"Update Available for {requestedFile.Name}", System.Windows.Forms.ToolTipIcon.None);
                                    await Task.Delay(6000);
                                    break;
                                case "automaticUpdate":
                                    switch (requestedFile.Name)
                                    {
                                        case "s1.grandtheftarma.Life.Altis.pbo":
                                            await Download.FileAsync(Data.FileIdArray[0], Data.CtsStopDownloading.Token);
                                            break;
                                        case "s2.grandtheftarma.Life.Altis.pbo":
                                            await Download.FileAsync(Data.FileIdArray[1], Data.CtsStopDownloading.Token);
                                            break;
                                        case "s3.grandtheftarma.Conflict.Tanoa.pbo":
                                            await Download.FileAsync(Data.FileIdArray[2], Data.CtsStopDownloading.Token);
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    //5 min
                    await Task.Delay(300000);
                }
            }
        }
    }
}