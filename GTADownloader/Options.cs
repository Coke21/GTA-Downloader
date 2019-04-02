using Microsoft.Win32;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace GTADownloader
{
    class Options
    {
        private static MainWindow win = (MainWindow)System.Windows.Application.Current.MainWindow;

        public static async Task Choose (string whichOption)
        {
            RegistryKey keyStartUp = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            RegistryKey keyDeleteValue = Registry.CurrentUser.OpenSubKey(@"Software\GTAProgram", true);
            RegistryKey keyRemoveSubKey = Registry.CurrentUser.OpenSubKey(@"Software\", true);

            switch (whichOption)
            {
                case "startUp":
                    keyStartUp.SetValue("GTADownloader", System.Reflection.Assembly.GetExecutingAssembly().Location);
                    break;
                case "startUpUnCheck":
                    keyStartUp.DeleteValue("GTADownloader");
                    break;
                case "runHidden":
                    win.StartUpCheckBox.IsChecked = true;
                    win.StartUpCheckBox.IsEnabled = false;
                    Registry.SetValue($"{FileData.programRegeditPath}", "Run Hidden", "Yes");
                    break;
                case "runHiddenUnCheck":
                    win.StartUpCheckBox.IsEnabled = true;
                    keyDeleteValue.DeleteValue("Run Hidden");
                    break;
                case "runTSAuto":
                    Registry.SetValue($"{FileData.programRegeditPath}", "Run TS Auto", "On");
                    break;
                case "runTSAutoUnCheck":
                    keyDeleteValue.DeleteValue("Run TS Auto");
                    break;
                case "S1Altis":
                    Registry.SetValue($"{FileData.programRegeditPath}", "S1Altis", "On");
                    FileData.missionFileListID.Add(FileData.fileIDArray[0]);
                    break;
                case "S1AltisUnCheck":
                    keyDeleteValue.DeleteValue("S1Altis");
                    FileData.missionFileListID.Remove(FileData.fileIDArray[0]);
                    break;
                case "S2Altis":
                    Registry.SetValue($"{FileData.programRegeditPath}", "S2Altis", "On");
                    FileData.missionFileListID.Add(FileData.fileIDArray[1]);
                    break;
                case "S2AltisUnCheck":
                    keyDeleteValue.DeleteValue("S2Altis");
                    FileData.missionFileListID.Remove(FileData.fileIDArray[1]);
                    break;
                case "S3Tanoa":
                    Registry.SetValue($"{FileData.programRegeditPath}", "S3Tanoa", "On");
                    FileData.missionFileListID.Add(FileData.fileIDArray[2]);
                    break;
                case "S3TanoaUnCheck":
                    keyDeleteValue.DeleteValue("S3Tanoa");
                    FileData.missionFileListID.Remove(FileData.fileIDArray[2]);
                    break;
                case "S3Malden":
                    Registry.SetValue($"{FileData.programRegeditPath}", "S3Malden", "On");
                    FileData.missionFileListID.Add(FileData.fileIDArray[3]);
                    break;
                case "S3MaldenUnCheck":
                    keyDeleteValue.DeleteValue("S3Malden");
                    FileData.missionFileListID.Remove(FileData.fileIDArray[3]);
                    break;
                case "notification":
                    Registry.SetValue($"{FileData.programRegeditPath}", "Notification", "On");
                    await CheckForUpdate.TypeOfNotificationAsync("notification", FileData.ctsNotification.Token);
                    break;
                case "notificationUnCheck":
                    keyDeleteValue.DeleteValue("Notification");
                    FileData.ctsNotification.Cancel();
                    FileData.ctsNotification.Dispose();
                    FileData.ctsNotification = new CancellationTokenSource();

                    FileData.ButtonsOption("optionsCheckBoxOff");
                    break;
                case "automaticUpdate":
                    Registry.SetValue($"{FileData.programRegeditPath}", "Automatic Update", "On");
                    await CheckForUpdate.TypeOfNotificationAsync("automaticUpdate", FileData.ctsAutomaticUpdate.Token);
                    break;
                case "automaticUpdateUnCheck":
                    keyDeleteValue.DeleteValue("Automatic Update");
                    FileData.ctsAutomaticUpdate.Cancel();
                    FileData.ctsAutomaticUpdate.Dispose();
                    FileData.ctsAutomaticUpdate = new CancellationTokenSource();

                    FileData.ButtonsOption("optionsCheckBoxOff");
                    break;
                case "maxSpeed":
                    Registry.SetValue($"{FileData.programRegeditPath}", "Download Speed", "Max");
                    Download.downloadSpeed = "maxSpeed";
                    break;
                case "normalSpeed":
                    Registry.SetValue($"{FileData.programRegeditPath}", "Download Speed", "Normal");
                    Download.downloadSpeed = "normalSpeed";
                    break;
                case "removeRegistry":
                    try
                    {
                        FileData.ButtonsOption("optionsCheckBoxOff");
                        FileData.ButtonsOption("deleteChangesToRegistry");

                        keyRemoveSubKey.DeleteSubKey("GTAProgram");
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
            if (startupValue != null) win.StartUpCheckBox.IsChecked = true;

            object runMinimizedValue = Registry.GetValue($"{FileData.programRegeditPath}", "Run Hidden", null);
            if (runMinimizedValue == null) win.WindowState = WindowState.Normal;
            else
            {
                win.HiddenCheckBox.IsChecked = true;
                win.ShowInTaskbar = false;
                win.Hide();
            }

            object runTSAutoValue = Registry.GetValue($"{FileData.programRegeditPath}", "Run TS Auto", null);
            if (runTSAutoValue != null)
            {
                win.RunTSAuto.IsChecked = true;

                Process[] process = Process.GetProcessesByName("ts3client_win64");
                if (process.Length == 0) Join.Server("joinTS", false);
            }

            object notificationValue = Registry.GetValue($"{FileData.programRegeditPath}", "Notification", null);
            if (notificationValue != null) win.NotificationCheckBox.IsChecked = true;

            string downloadSpeed = "";
            try
            {
                downloadSpeed = Registry.GetValue($"{FileData.programRegeditPath}", "Download Speed", "Max").ToString();
            }
            catch (NullReferenceException)
            {
                win.MaxSpeedButton.IsChecked = true;
            }
            finally
            {
                if (downloadSpeed == "Max") win.MaxSpeedButton.IsChecked = true;
                else if (downloadSpeed == "Normal") win.NormalSpeedButton.IsChecked = true;
            }

            object automaticUpdateValue = Registry.GetValue($"{FileData.programRegeditPath}", "Automatic Update", null);
            if (automaticUpdateValue != null) win.AutomaticUpdateCheckBox.IsChecked = true;

            object S1AltisValue = Registry.GetValue($"{FileData.programRegeditPath}", "S1Altis", null);
            if (S1AltisValue != null) win.S1AltisCheckBox.IsChecked = true;

            object S2AltisValue = Registry.GetValue($"{FileData.programRegeditPath}", "S2Altis", null);
            if (S2AltisValue != null) win.S2AltisCheckBox.IsChecked = true;

            object S3TanoaValue = Registry.GetValue($"{FileData.programRegeditPath}", "S3Tanoa", null);
            if (S3TanoaValue != null) win.S3TanoaCheckBox.IsChecked = true;

            object S3MaldenValue = Registry.GetValue($"{FileData.programRegeditPath}", "S3Malden", null);
            if (S3MaldenValue != null) win.S3MaldenCheckBox.IsChecked = true;
        }   
    }
}
