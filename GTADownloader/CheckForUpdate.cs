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
        public static async Task UpdateAsync(CancellationToken cancellationToken)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            await Task.Run(() =>
            {
                foreach (var fileID in Data.fileIDArray)
                {
                    var request = Data.GetFileRequest(fileID, "size, name");

                    string fileName = request.Execute().Name;
                    long? fileSizeOnline = request.Execute().Size;

                    long fileSizeOnComputer = 0;

                    string fileLoc = Path.Combine(Data.getMissionFolderPath, fileName);

                    if (cancellationToken.IsCancellationRequested) return;

                    try
                    {
                        fileSizeOnComputer = new FileInfo(fileLoc).Length;
                    }
                    catch (FileNotFoundException)
                    {
                        win.Dispatcher.Invoke(() => updateNotiMF("missing", Brushes.Black));
                    }
                    if (File.Exists(fileLoc))
                        if (fileSizeOnline == fileSizeOnComputer)
                            win.Dispatcher.Invoke(() => updateNotiMF("updated", Brushes.ForestGreen));
                        else
                            win.Dispatcher.Invoke(() => updateNotiMF("outdated", Brushes.Red));

                    void updateNotiMF(string text, SolidColorBrush colour) => win.TextTopOperationNotice.Inlines.Add(new Run($"{fileName} is {text}!\n") { Foreground = colour });
                }

                long? gtaSizeProgramOnline = Data.GetFileRequest(Data.programID, "size").Execute().Size;
                long gtaSizeProgramOnComputer = new FileInfo(Data.getProgramFolderPath + Data.getProgramName).Length;

                if (gtaSizeProgramOnline == gtaSizeProgramOnComputer)
                    win.Dispatcher.Invoke(() => UpdateNotiProgram("updated.", Brushes.ForestGreen));
                else
                {
                    win.Dispatcher.Invoke(() => UpdateNotiProgram("outdated", Brushes.Red));
                    win.Dispatcher.Invoke(() => win.ProgramUpdateName.Visibility = Visibility.Visible);
                }
                void UpdateNotiProgram(string text, SolidColorBrush colour) => win.TextTopOperationProgramNotice.Inlines.Add(new Run($"The GTA program is {text}!") { Foreground = colour });
            });
        }
        public static async Task TypeOfNotificationAsync(string whichOption, CancellationToken cancellationToken)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            Data.notifyIcon.BalloonTipClicked += (sender, e) => Data.NotifyIconBalloonTipClicked();

            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (Data.missionFileListID.Count > 0)
                    {
                        foreach (var fileID in Data.missionFileListID.ToList())
                        {
                            var request = Data.GetFileRequest(fileID, "size, name");
                            long? fileSizeOnline = request.Execute().Size;
                            string fileName = request.Execute().Name;

                            long fileSizeOnComputer = 0;

                            try
                            {
                                string fileLoc = Path.Combine(Data.getMissionFolderPath, fileName);
                                fileSizeOnComputer = new FileInfo(fileLoc).Length;
                            }
                            catch (FileNotFoundException)
                            {
                            }
                            if (fileSizeOnComputer != fileSizeOnline)
                            {
                                switch (whichOption)
                                {
                                    case "notification":
                                        Data.notifyIcon.ShowBalloonTip(4000, "Download now!", $"Update Available for {fileName}", System.Windows.Forms.ToolTipIcon.None);
                                        await Task.Delay(6000);
                                        break;
                                    case "automaticUpdate":
                                        switch (fileName)
                                        {
                                            case "s1.grandtheftarma.Life.Altis.pbo":
                                                await win.Dispatcher.Invoke(async () => await Download.FileAsync(Data.fileIDArray[0], Data.ctsStopDownloading.Token));
                                                break;
                                            case "s2.grandtheftarma.Life.Altis.pbo":
                                                await win.Dispatcher.Invoke(async () => await Download.FileAsync(Data.fileIDArray[1], Data.ctsStopDownloading.Token));
                                                break;
                                            case "s3.grandtheftarma.Conflict.Tanoa.pbo":
                                                await win.Dispatcher.Invoke(async () => await Download.FileAsync(Data.fileIDArray[2], Data.ctsStopDownloading.Token));
                                                break;
                                            case "s3.grandtheftarma.BattleRoyale.Malden.pbo":
                                                await win.Dispatcher.Invoke(async () => await Download.FileAsync(Data.fileIDArray[3], Data.ctsStopDownloading.Token));
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
            });
        }
    }
}