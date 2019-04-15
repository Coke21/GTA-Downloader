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
                foreach (var file in Data.fileIDArray)
                {
                    var request = Data.service.Files.Get(file);
                    request.Fields = "size, name";

                    long? fileSizeOnline = request.Execute().Size;
                    long fileSizeOnComputer = 0;
                    string fileName = request.Execute().Name;

                    string fileLoc = Path.Combine(Data.missionFolderPath, fileName);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        win.Dispatcher.Invoke(() =>
                        {
                            win.TextTopOperationNotice.Text = "";
                            win.TextTopOperationProgramNotice.Text = "";
                        });
                        break;
                    }

                    try
                    {
                        fileSizeOnComputer = new FileInfo(fileLoc).Length;
                    }
                    catch (FileNotFoundException)
                    {
                        win.Dispatcher.Invoke(() => win.TextTopOperationNotice.Inlines.Add(new Run($"{fileName} is missing!\n") { Foreground = Brushes.Black }));
                    }
                    if (File.Exists(fileLoc))
                        if (fileSizeOnline == fileSizeOnComputer)
                            win.Dispatcher.Invoke(() => win.TextTopOperationNotice.Inlines.Add(new Run($"{fileName} is updated.\n") { Foreground = Brushes.ForestGreen }));
                        else
                            win.Dispatcher.Invoke(() => win.TextTopOperationNotice.Inlines.Add(new Run($"{fileName} is outdated!\n") { Foreground = Brushes.Red }));
                }

                var requestForProgram = Data.service.Files.Get(Data.programID);
                requestForProgram.Fields = "size";
                long? gtaProgramOnlineSize = requestForProgram.Execute().Size;

                long gtaProgramOnComputerSize = new FileInfo(Data.programPathExe + Data.programName).Length;

                win.Dispatcher.Invoke(() =>
                {
                    if (gtaProgramOnlineSize == gtaProgramOnComputerSize)
                        UpdateNotiProgram("The GTA program is updated.", Brushes.ForestGreen);
                    else
                        UpdateNotiProgram("The GTA program is outdated!", Brushes.Red);
                });
                void UpdateNotiProgram(string text, SolidColorBrush colour)
                {
                    win.TextTopOperationProgramNotice.Text = text;
                    win.TextTopOperationProgramNotice.Foreground = colour;
                    if (cancellationToken.IsCancellationRequested) win.TextTopOperationProgramNotice.Text = "";
                }
            });
        }
        public static async void NotifyIconBalloonTipClicked(object sender, EventArgs e, bool stopOnStart = true, bool updateFiles = true)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            if (stopOnStart) win.StopOnStart();
            win.WindowState = WindowState.Normal;
            win.Show();
            if (updateFiles) await UpdateAsync(Data.ctsOnStart.Token);
        }
        public static async Task TypeOfNotificationAsync(string whichOption, CancellationToken cancellationToken)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            Data.notifyIcon.BalloonTipClicked += (sender, e) => NotifyIconBalloonTipClicked(sender, e);

            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (Data.missionFileListID.Count > 0)
                    {
                        foreach (var file in Data.missionFileListID.ToList())
                        {
                            var request = Data.service.Files.Get(file);
                            request.Fields = "size, name";

                            long fileSizeOnComputer = 0;
                            long? fileSizeOnline = request.Execute().Size;
                            string fileName = request.Execute().Name;

                            try
                            {
                                string fileLoc = Path.Combine(Data.missionFolderPath, fileName);
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