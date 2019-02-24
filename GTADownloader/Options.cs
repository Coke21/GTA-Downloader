using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace GTADownloader
{
    class Options
    {
        public static MainWindow win = (MainWindow)System.Windows.Application.Current.MainWindow;

        public static void Choose (string whichOption)
        {
            RegistryKey keyRun = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            RegistryKey keyRunMinimized = Registry.CurrentUser.OpenSubKey(@"Software\GTAProgram", true);
            RegistryKey keyRemove = Registry.CurrentUser.OpenSubKey(@"Software\", true);

            switch (whichOption)
            {
                case "startUp":
                    keyRun.SetValue("GTADownloader", System.Reflection.Assembly.GetExecutingAssembly().Location);
                    break;
                case "startUpUnCheck":
                    keyRun.DeleteValue("GTADownloader");
                    break;
                case "runMinimized":
                    win.StartUpCheckBox.IsChecked = true;
                    win.StartUpCheckBox.IsEnabled = false;
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Run Minimized", "Yes");
                    break;
                case "runMinimizedUnCheck":
                    win.StartUpCheckBox.IsEnabled = true;
                    keyRunMinimized.DeleteValue("Run Minimized");
                    break;
                case "S1Altis":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S1Altis", "On");
                    break;
                case "S1AltisUnCheck":
                    keyRunMinimized.DeleteValue("S1Altis");
                    break;
                case "S2Altis":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S2Altis", "On");
                    break;
                case "S2AltisUnCheck":
                    keyRunMinimized.DeleteValue("S2Altis");
                    break;
                case "S3Altis":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Altis", "On");
                    break;
                case "S3AltisUnCheck":
                    keyRunMinimized.DeleteValue("S3Altis");
                    break;
                case "S1Tanoa":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S1Tanoa", "On");
                    break;
                case "S1TanoaUnCheck":
                    keyRunMinimized.DeleteValue("S1Tanoa");
                    break;
                case "S2Tanoa":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S2Tanoa", "On");
                    break;
                case "S2TanoaUnCheck":
                    keyRunMinimized.DeleteValue("S2Tanoa");
                    break;
                case "S3Tanoa":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Tanoa", "On");
                    break;
                case "S3TanoaUnCheck":
                    keyRunMinimized.DeleteValue("S3Tanoa");
                    break;
                case "notification":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Notification", "On");
                    break;
                case "notificationUnCheck":
                    keyRunMinimized.DeleteValue("Notification");
                    break;
                case "automaticUpdate":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Automatic Update", "On");
                    break;
                case "automaticUpdateUnCheck":
                    keyRunMinimized.DeleteValue("Automatic Update");
                    break;
                case "removeRegistry":
                    try
                    {
                        keyRemove.DeleteSubKey("GTAProgram");
                        System.Windows.MessageBox.Show("All changes made to your registry have been deleted.", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    catch (ArgumentException exc)
                    {
                        System.Windows.MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
            }
        }
        public static void UpdateCheckBoxes()
        {
            object startupValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "GTADownloader", null);
            if (startupValue == null)
                win.StartUpCheckBox.IsChecked = false;
            else
                win.StartUpCheckBox.IsChecked = true;

            object runMinimizedValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Run Minimized", null);
            if (runMinimizedValue == null)
            {
                win.MinimizedCheckBox.IsChecked = false;
                win.WindowState = WindowState.Normal;
            }
            else
            {
                win.MinimizedCheckBox.IsChecked = true;
                win.WindowState = WindowState.Minimized;
            }

            object notificationValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Notification", null);
            if (notificationValue == null)
                win.NotificationCheckBox.IsChecked = false;
            else
                win.NotificationCheckBox.IsChecked = true;

            object automaticUpdateValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Automatic Update", null);
            if (automaticUpdateValue == null)
                win.AutomaticUpdateCheckBox.IsChecked = false;
            else
                win.AutomaticUpdateCheckBox.IsChecked = true;

            object S1AltisValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S1Altis", null);
            if (S1AltisValue == null)
                win.S1AltisCheckBox.IsChecked = false;
            else
                win.S1AltisCheckBox.IsChecked = true;

            object S2AltisValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S2Altis", null);
            if (S2AltisValue == null)
                win.S2AltisCheckBox.IsChecked = false;
            else
                win.S2AltisCheckBox.IsChecked = true;

            object S3AltisValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Altis", null);
            if (S3AltisValue == null)
                win.S3AltisCheckBox.IsChecked = false;
            else
                win.S3AltisCheckBox.IsChecked = true;

            object S1TanoaValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S1Tanoa", null);
            if (S1TanoaValue == null)
                win.S1TanoaCheckBox.IsChecked = false;
            else
                win.S1TanoaCheckBox.IsChecked = true;

            object S2TanoaValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S2Tanoa", null);
            if (S2TanoaValue == null)
                win.S2TanoaCheckBox.IsChecked = false;
            else
                win.S2TanoaCheckBox.IsChecked = true;

            object S3TanoaValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Tanoa", null);
            if (S3TanoaValue == null)
                win.S3TanoaCheckBox.IsChecked = false;
            else
                win.S3TanoaCheckBox.IsChecked = true;
        }
        public static async Task NotificationAsync(string whichOption, CancellationToken cancellationToken)
        {
            await Task.Run(async() =>
            {
                while (true)
                {
                    if (FileData.missionFileListName.Count > 0)
                    {
                        foreach (var file in FileData.missionFileListName.Zip(FileData.missionFileListID, Tuple.Create).ToList())
                        {
                            var request = FileData.service.Files.Get(file.Item2);
                            request.Fields = "size";

                            long fileSizeOnComputer = 0;
                            long? fileSizeOnline = request.Execute().Size;

                            try
                            {
                                string fileLoc = Path.Combine(FileData.folderPath, file.Item1);
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
                                        if (cancellationToken.IsCancellationRequested) break;
                                        FileData.notifyIcon.ShowBalloonTip(3000, $"Update Available for {file.Item1}", "Download now!", ToolTipIcon.None);
                                        await Task.Delay(5000);
                                        break;
                                    case "automaticUpdate":
                                        if (cancellationToken.IsCancellationRequested) goto End;
                                            
                                        await win.Dispatcher.BeginInvoke((Action)(async () =>
                                        {
                                            if (!Download.isExecuted)
                                            {
                                                switch (file.Item1)
                                                {
                                                    case "s1.grandtheftarma.Altis.pbo":
                                                        await Download.FileAsync(FileData.fileIDArray[0], FileData.fileNameArray[0]);
                                                        break;
                                                    case "s2.grandtheftarma.Altis.pbo":
                                                        await Download.FileAsync(FileData.fileIDArray[1], FileData.fileNameArray[1]);
                                                        break;
                                                    case "s3.grandtheftarma.Altis.pbo":
                                                        await Download.FileAsync(FileData.fileIDArray[2], FileData.fileNameArray[2]);
                                                        break;
                                                    case "s1.grandtheftarma.Tanoa.pbo":
                                                        await Download.FileAsync(FileData.fileIDArray[3], FileData.fileNameArray[3]);
                                                        break;
                                                    case "s2.grandtheftarma.Tanoa.pbo":
                                                        await Download.FileAsync(FileData.fileIDArray[4], FileData.fileNameArray[4]);
                                                        break;
                                                    case "s3.grandtheftarma.Tanoa.pbo":
                                                        await Download.FileAsync(FileData.fileIDArray[5], FileData.fileNameArray[5]);
                                                        break;
                                                }
                                            }
                                        }));
                                        break;
                                }
                            }
                        }
                    }
                await Task.Delay(5000);
                }
            End:;
            });
        }
    }
}
