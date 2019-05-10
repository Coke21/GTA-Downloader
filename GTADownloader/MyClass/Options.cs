using Microsoft.Win32;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace GTADownloader
{
    class Options
    {
        private static MainWindow win = (MainWindow)Application.Current.MainWindow;
        public static async Task Choose (string whichOption)
        {
            RegistryKey keyStartUp = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
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
                    FileOperation.EditFileLine("Run Hidden=0", "Run Hidden=1");
                    break;
                case "runHiddenUnCheck":
                    win.StartUpCheckBox.IsEnabled = true;
                    FileOperation.EditFileLine("Run Hidden=1", "Run Hidden=0");
                    win.ShowInTaskbar = true;
                    break;
                case "runTSAuto":
                    FileOperation.EditFileLine("Run TS Auto=0", "Run TS Auto=1");
                    break;
                case "runTSAutoUnCheck":
                    FileOperation.EditFileLine("Run TS Auto=1", "Run TS Auto=0");
                    break;
                case "S1Altis":
                    FileOperation.EditFileLine("S1Altis=0", "S1Altis=1");
                    Data.missionFileListID.Add(Data.fileIDArray[0]);
                    break;
                case "S1AltisUnCheck":
                    FileOperation.EditFileLine("S1Altis=1", "S1Altis=0");
                    Data.missionFileListID.Remove(Data.fileIDArray[0]);
                    break;
                case "S2Altis":
                    FileOperation.EditFileLine("S2Altis=0", "S2Altis=1");
                    Data.missionFileListID.Add(Data.fileIDArray[1]);
                    break;
                case "S2AltisUnCheck":
                    FileOperation.EditFileLine("S2Altis=1", "S2Altis=0");
                    Data.missionFileListID.Remove(Data.fileIDArray[1]);
                    break;
                case "S3Tanoa":
                    FileOperation.EditFileLine("S3Tanoa=0", "S3Tanoa=1");
                    Data.missionFileListID.Add(Data.fileIDArray[2]);
                    break;
                case "S3TanoaUnCheck":
                    FileOperation.EditFileLine("S3Tanoa=1", "S3Tanoa=0");
                    Data.missionFileListID.Remove(Data.fileIDArray[2]);
                    break;
                case "S3Malden":
                    FileOperation.EditFileLine("S3Malden=0", "S3Malden=1");
                    Data.missionFileListID.Add(Data.fileIDArray[3]);
                    break;
                case "S3MaldenUnCheck":
                    FileOperation.EditFileLine("S3Malden=1", "S3Malden=0");
                    Data.missionFileListID.Remove(Data.fileIDArray[3]);
                    break;
                case "notification":
                    FileOperation.EditFileLine("Notification=0", "Notification=1");
                    await CheckForUpdate.TypeOfNotificationAsync("notification", Data.ctsNotification.Token);
                    break;
                case "notificationUnCheck":
                    FileOperation.EditFileLine("Notification=1", "Notification=0");

                    Data.ctsNotification.Cancel();
                    Data.ctsNotification.Dispose();
                    Data.ctsNotification = new CancellationTokenSource();

                    DataHelper.ButtonsOption("optionsCheckBoxOff");
                    break;
                case "automaticUpdate":
                    FileOperation.EditFileLine("Automatic Update=0", "Automatic Update=1");
                    await CheckForUpdate.TypeOfNotificationAsync("automaticUpdate", Data.ctsAutomaticUpdate.Token);
                    break;
                case "automaticUpdateUnCheck":
                    FileOperation.EditFileLine("Automatic Update=1", "Automatic Update=0");

                    Data.ctsAutomaticUpdate.Cancel();
                    Data.ctsAutomaticUpdate.Dispose();
                    Data.ctsAutomaticUpdate = new CancellationTokenSource();

                    DataHelper.ButtonsOption("optionsCheckBoxOff");
                    break;
                case "maxSpeed":
                    FileOperation.EditFileLine("Download Speed=0", "Download Speed=1");
                    Download.DownloadSpeed = "maxSpeed";
                    break;
                case "normalSpeed":
                    FileOperation.EditFileLine("Download Speed=1", "Download Speed=0");
                    Download.DownloadSpeed = "normalSpeed";
                    break;
                case "removeRegistry":
                    try
                    {
                        DataHelper.ButtonsOption("optionsCheckBoxOff");
                        DataHelper.ButtonsOption("deleteChangesToRegistry");

                        keyRemoveSubKey.DeleteSubKey("GTAProgram");
                        MessageBox.Show("All changes made to your registry have been deleted.", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    catch (ArgumentException exc)
                    {
                        MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
            }
        }
        public static void UpdateCheckBoxes()
        {
            object startupValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "GTADownloader", null);
            if (startupValue != null) win.StartUpCheckBox.IsChecked = true;

            var data = File
            .ReadAllLines(Data.getConfigFilePath)
            .Select(x => x.Split('='))
            .Where(x => x.Length > 1)
            .ToDictionary(x => x[0].Trim(), x => x[1]);

            try
            {
                if (data["Default Lv channel"].Length > 0) win.insertTSChannelName.Text = data["Default Lv channel"];
                if (data["Default Lv password"].Length > 0) win.insertTSChannelPasswordName.Text = data["Default Lv password"];

                if (data["Run Hidden"] == "1")
                {
                    win.HiddenCheckBox.IsChecked = true;
                    win.ShowInTaskbar = false;
                    win.Hide();
                    Data.notifyIcon.ShowBalloonTip(4000, "Reminder!", $"The program is running in the background!", System.Windows.Forms.ToolTipIcon.None);
                    Data.notifyIcon.BalloonTipClicked += (sender, e) => DataHelper.NotifyIconBalloonTipClicked(false, false);
                }
                if (data["Run TS Auto"] == "1")
                {
                    win.RunTSAuto.IsChecked = true;

                    Process[] process = Process.GetProcessesByName("ts3client_win64");
                    if (process.Length == 0) Join.Server("joinTS", false);
                }

                if (data["S1Altis"] == "1") win.S1AltisCheckBox.IsChecked = true;

                if (data["S2Altis"] == "1") win.S2AltisCheckBox.IsChecked = true;

                if (data["S3Tanoa"] == "1") win.S3TanoaCheckBox.IsChecked = true;

                if (data["S3Malden"] == "1") win.S3MaldenCheckBox.IsChecked = true;

                if (data["Notification"] == "1") win.NotificationCheckBox.IsChecked = true;

                if (data["Automatic Update"] == "1") win.AutomaticUpdateCheckBox.IsChecked = true;

                switch (data["Download Speed"])
                {
                    case "1":
                        win.MaxSpeedButton.IsChecked = true;
                        break;
                    case "0":
                        win.NormalSpeedButton.IsChecked = true;
                        break;
                }
            }
            catch (KeyNotFoundException)
            {
                File.Delete(Data.getConfigFilePath);
                FileOperation.IsFilePresent("config");
            }
        }   
    }
}
