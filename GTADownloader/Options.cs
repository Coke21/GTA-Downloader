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
        private static MainWindow win = (MainWindow)System.Windows.Application.Current.MainWindow;

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
                case "S3Tanoa":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Tanoa", "On");
                    break;
                case "S3TanoaUnCheck":
                    keyRunMinimized.DeleteValue("S3Tanoa");
                    break;
                case "S3Malden":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Malden", "On");
                    break;
                case "S3MaldenUnCheck":
                    keyRunMinimized.DeleteValue("S3Malden");
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
                case "maxSpeed":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Download Speed", "Max");
                    break;
                case "normalSpeed":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Download Speed", "Normal");
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

            string downloadSpeed = "";
            try
            {
                downloadSpeed = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Download Speed", "Max").ToString();
            }
            catch (NullReferenceException)
            {
                win.MaxSpeedButton.IsChecked = true;
            }
            finally
            {
                if (downloadSpeed == "Max")
                    win.MaxSpeedButton.IsChecked = true;
                else if (downloadSpeed == "Normal")
                    win.NormalSpeedButton.IsChecked = true;
            }

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

            object S3TanoaValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Tanoa", null);
            if (S3TanoaValue == null)
                win.S3TanoaCheckBox.IsChecked = false;
            else
                win.S3TanoaCheckBox.IsChecked = true;

            object S3MaldenValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Malden", null);
            if (S3MaldenValue == null)
                win.S3MaldenCheckBox.IsChecked = false;
            else
                win.S3MaldenCheckBox.IsChecked = true;
        }   
        public static async Task TypeOfNotification(string whichOption, CancellationToken cancellationToken)
        {
            await Task.Run(async() =>
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
                                        FileData.notifyIcon.ShowBalloonTip(4000, $"Update Available for {fileName}", "Download now!", ToolTipIcon.None);
                                        await Task.Delay(5000);
                                        break;
                                    case "automaticUpdate":
                                        if (cancellationToken.IsCancellationRequested) goto Abort;

                                        if (!Download.isExecuted)
                                        {
                                            switch (fileName)
                                            {
                                                case "s1.grandtheftarma.Life.Altis.pbo":
                                                    await Download.FileAsync(FileData.fileIDArray[0]);
                                                    break;
                                                case "s2.grandtheftarma.Life.Altis.pbo":
                                                    await Download.FileAsync(FileData.fileIDArray[1]);
                                                    break;
                                                case "s3.grandtheftarma.Conflict.Tanoa.pbo":
                                                    await Download.FileAsync(FileData.fileIDArray[2]);
                                                    break;
                                                case "s3.grandtheftarma.BattleRoyale.Malden.pbo":
                                                    await Download.FileAsync(FileData.fileIDArray[3]);
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
