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
            foreach (var file in Data.fileIDArray.Zip(Data.fileNameArray, Tuple.Create))
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

            var requestedProgram = await DataHelper.GetFileRequest(Data.programID, "size").ExecuteAsync();
            long gtaSizeProgramOnComputer = new FileInfo(DataProperties.GetProgramFolderPath + DataProperties.GetProgramName).Length;

            if (requestedProgram.Size == gtaSizeProgramOnComputer)
                UpdateNotiProgram("updated", Brushes.ForestGreen);
            else
            {
                UpdateNotiProgram("outdated", Brushes.Red);
                Win.ProgramUpdateName.Visibility = Visibility.Visible;
                Win.ReadChangelogName.Visibility = Visibility.Visible;

                if (Win.GTAUpdateCheckBox.IsChecked.Value)
                {
                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("A new update for GTA program has been detected. Download it?", "Update", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                        await Download.FileAsync(Data.programID, Data.ctsStopDownloading.Token, "programUpdate");
                }
            }
        }
        private static void UpdateNotiMf(string fileName, string status, SolidColorBrush colour) => Win.TextTopOperationNotice.Inlines.Add(new Run($"{fileName} is {status}!\n") { Foreground = colour });
        private static void UpdateNotiProgram(string status, SolidColorBrush colour) => Win.TextTopOperationProgramNotice.Inlines.Add(new Run($"The GTA program is {status}!") { Foreground = colour });

        public static async Task TypeOfNotificationAsync(string whichOption, CancellationToken cancellationToken)
        {
            Data.notifyIcon.BalloonTipClicked += (sender, e) => Notification.NotifyIconBalloonTipClicked();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (Data.missionFileListID.Count > 0)
                {
                    foreach (var fileID in Data.missionFileListID.ToList())
                    {
                        var requestedFile = await DataHelper.GetFileRequest(fileID, "size, name").ExecuteAsync();

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
                                    Data.notifyIcon.ShowBalloonTip(4000, "Download now!", $"Update Available for {requestedFile.Name}", System.Windows.Forms.ToolTipIcon.None);
                                    await Task.Delay(6000);
                                    break;
                                case "automaticUpdate":
                                    switch (requestedFile.Name)
                                    {
                                        case "s1.grandtheftarma.Life.Altis.pbo":
                                            await Download.FileAsync(Data.fileIDArray[0], Data.ctsStopDownloading.Token);
                                            break;
                                        case "s2.grandtheftarma.Life.Altis.pbo":
                                            await Download.FileAsync(Data.fileIDArray[1], Data.ctsStopDownloading.Token);
                                            break;
                                        case "s3.grandtheftarma.Conflict.Tanoa.pbo":
                                            await Download.FileAsync(Data.fileIDArray[2], Data.ctsStopDownloading.Token);
                                            break;
                                        case "s2.grandtheftarma.Life.Enoch.pbo":
                                            await Download.FileAsync(Data.fileIDArray[3], Data.ctsStopDownloading.Token);
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