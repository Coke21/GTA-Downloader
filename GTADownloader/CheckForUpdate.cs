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
        private static long gtaProgramOnComputerSize = new FileInfo(Data.programPath + Data.programName).Length;

        public static async Task <bool> UpdateAsync(CancellationToken cancellationToken)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            await Task.Run(() =>
            {
                var requestForProgram = Data.service.Files.Get(Data.programID);
                requestForProgram.Fields = "size";
                long? gtaProgramOnlineSize = requestForProgram.Execute().Size;

                foreach (var file in Data.fileIDArray)
                {
                    var request = Data.service.Files.Get(file);
                    request.Fields = "size, name";

                    long? fileSizeOnline = request.Execute().Size;
                    long fileSizeOnComputer = 0;
                    string fileName = request.Execute().Name;

                    string fileLoc = Path.Combine(Data.folderPath, fileName);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        win.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            win.TextTopOperationNotice.Text = "";
                            win.TextTopOperationProgramNotice.Text = "";
                        }));
                        break;
                    }
                    try
                    {
                        fileSizeOnComputer = new FileInfo(fileLoc).Length;
                    }
                    catch (FileNotFoundException)
                    {
                        win.Dispatcher.BeginInvoke((Action)(() => win.TextTopOperationNotice.Inlines.Add(new Run($"{fileName} is missing!\n") { Foreground = Brushes.Black })));
                    }
                    if (File.Exists(fileLoc))
                        if (fileSizeOnline == fileSizeOnComputer)
                            win.Dispatcher.BeginInvoke((Action)(() => win.TextTopOperationNotice.Inlines.Add(new Run($"{fileName} is updated.\n") { Foreground = Brushes.ForestGreen })));
                        else
                            win.Dispatcher.BeginInvoke((Action)(() => win.TextTopOperationNotice.Inlines.Add(new Run($"{fileName} is outdated!\n") { Foreground = Brushes.Red })));
                }
                if (gtaProgramOnlineSize == gtaProgramOnComputerSize)
                    win.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        win.TextTopOperationProgramNotice.Text = "The GTA program is updated.";
                        win.TextTopOperationProgramNotice.Foreground = Brushes.ForestGreen;
                        if (cancellationToken.IsCancellationRequested) win.TextTopOperationProgramNotice.Text = "";
                    }));
                else
                    win.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        win.TextTopOperationProgramNotice.Text = "The GTA program is outdated!";
                        win.TextTopOperationProgramNotice.Foreground = Brushes.Red;
                        if (cancellationToken.IsCancellationRequested) win.TextTopOperationProgramNotice.Text = "";
                    }));
            });
            return true;
        }

        public static async void NotifyIconBalloonTipClicked(object sender, EventArgs e, bool stopOnStart = true, bool updateFiles = true)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            bool result = stopOnStart ? win.StopOnStart() : false;
            win.WindowState = WindowState.Normal;
            win.Show();
            bool result2 = updateFiles ? await UpdateAsync(Data.ctsOnStart.Token) : false;
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
                                string fileLoc = Path.Combine(Data.folderPath, fileName);
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
                                        await Task.Delay(5000);
                                        break;
                                    case "automaticUpdate":
                                        if (!Download.isExecuted)
                                        {
                                            switch (fileName)
                                            {
                                                case "s1.grandtheftarma.Life.Altis.pbo":
                                                    await win.Dispatcher.BeginInvoke((Action)(async () => await Download.FileAsync(Data.fileIDArray[0], Data.ctsStopDownloading.Token)));
                                                    break;
                                                case "s2.grandtheftarma.Life.Altis.pbo":
                                                    await win.Dispatcher.BeginInvoke((Action)(async () => await Download.FileAsync(Data.fileIDArray[1], Data.ctsStopDownloading.Token)));
                                                    break;
                                                case "s3.grandtheftarma.Conflict.Tanoa.pbo":
                                                    await win.Dispatcher.BeginInvoke((Action)(async () => await Download.FileAsync(Data.fileIDArray[2], Data.ctsStopDownloading.Token)));
                                                    break;
                                                case "s3.grandtheftarma.BattleRoyale.Malden.pbo":
                                                    await win.Dispatcher.BeginInvoke((Action)(async () => await Download.FileAsync(Data.fileIDArray[3], Data.ctsStopDownloading.Token)));
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    await Task.Delay(5000);
                }
            });
        }
    }
}