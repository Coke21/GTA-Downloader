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
        private static long gtaProgramOnComputerSize = new FileInfo(FileData.programPath + FileData.programName).Length;

        public static async Task UpdateAsync(CancellationToken cancellationToken)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            await Task.Run(() =>
            {
                var requestForProgram = FileData.service.Files.Get(FileData.programID);
                requestForProgram.Fields = "size";
                long? gtaProgramOnlineSize = requestForProgram.Execute().Size;

                foreach (var file in FileData.fileIDArray)
                {
                    var request = FileData.service.Files.Get(file);
                    request.Fields = "size, name";

                    long? fileSizeOnline = request.Execute().Size;
                    long fileSizeOnComputer = 0;
                    string fileName = request.Execute().Name;

                    string fileLoc = Path.Combine(FileData.folderPath, fileName);

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
        }
        public static void TaskBar()
        {
            FileData.notifyIcon.Visible = true;
            FileData.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            FileData.notifyIcon.Click += NotifyIcon_BalloonTipClicked;
            FileData.notifyIcon.BalloonTipClicked += NotifyIcon_BalloonTipClicked;
            FileData.notifyIcon.Text = "GTA Mission Downloader";

            async void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
            {
                MainWindow win = (MainWindow)Application.Current.MainWindow;
                win.StopOnStart();
                win.WindowState = WindowState.Normal;
                win.Show();
                await UpdateAsync(FileData.ctsOnStart.Token);
            }
        }

        public static async Task TypeOfNotificationAsync(string whichOption, CancellationToken cancellationToken)
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (FileData.missionFileListID.Count > 0)
                    {
                        foreach (var file in FileData.missionFileListID.ToList())
                        {
                            var request = FileData.service.Files.Get(file);
                            request.Fields = "size, name";

                            long fileSizeOnComputer = 0;
                            long? fileSizeOnline = request.Execute().Size;
                            string fileName = request.Execute().Name;

                            try
                            {
                                string fileLoc = Path.Combine(FileData.folderPath, fileName);
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
                                        if (cancellationToken.IsCancellationRequested) goto Abort;
                                        FileData.notifyIcon.ShowBalloonTip(4000, $"Update Available for {fileName}", "Download now!", System.Windows.Forms.ToolTipIcon.None);
                                        await Task.Delay(5000);
                                        break;
                                    case "automaticUpdate":
                                        if (cancellationToken.IsCancellationRequested) goto Abort;

                                        if (!Download.isExecuted)
                                        {
                                            switch (fileName)
                                            {
                                                case "s1.grandtheftarma.Life.Altis.pbo":
                                                    await win.Dispatcher.BeginInvoke((Action)(async () => await Download.FileAsync(FileData.fileIDArray[0])));
                                                    break;
                                                case "s2.grandtheftarma.Life.Altis.pbo":
                                                    await win.Dispatcher.BeginInvoke((Action)(async () => await Download.FileAsync(FileData.fileIDArray[1])));
                                                    break;
                                                case "s3.grandtheftarma.Conflict.Tanoa.pbo":
                                                    await win.Dispatcher.BeginInvoke((Action)(async () => await Download.FileAsync(FileData.fileIDArray[2])));
                                                    break;
                                                case "s3.grandtheftarma.BattleRoyale.Malden.pbo":
                                                    await win.Dispatcher.BeginInvoke((Action)(async () => await Download.FileAsync(FileData.fileIDArray[3])));
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
            Abort:;
            });
        }
    }
}