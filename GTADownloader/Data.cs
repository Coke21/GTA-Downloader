using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace GTADownloader
{
    class Data
    {
        public static string programVersion = "1.0c";

        public static string[] fileIDArray = {"1KIzqR9NMBZoxcdibMZPxr13__azGdGye",
                                              "15Or16ZcPqSzGF6b41p7_IDGzI3SDGCnJ",
                                              "1ZJQBHLuMK3-OT-BRglVg83wE2jEMrZgD",
                                              "1f5kNp5Erfs20J3u4pT2zBZNpV3A_f3sI"};

        public static string programID = "1EHQqd72EELxE-GXFCS4urWzn_3fL5wI2";

        public static DriveService service = new DriveService(new BaseClientService.Initializer()
        {
            ApiKey = "xd",
        });

        public static string getMissionFolderPath = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/Arma 3/MPMissionsCache/";
        public static string getDataFolderPath = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/GTADownloader";
        public static string getListViewFilePath = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/GTADownloader/GTALvData.txt";
        public static string getConfigFilePath = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/GTADownloader/GTAData.txt";

        public static string getArma3EXEPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\bohemia interactive\arma 3", "main", string.Empty).ToString() + @"\arma3battleye";

        public static string getProgramFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\";
        public static string getProgramName = AppDomain.CurrentDomain.FriendlyName;

        public static List<string> missionFileListID = new List<string>();
        public static NotifyIcon notifyIcon = new NotifyIcon();

        public static CancellationTokenSource ctsOnStart = new CancellationTokenSource();
        public static CancellationTokenSource ctsNotification = new CancellationTokenSource();
        public static CancellationTokenSource ctsAutomaticUpdate = new CancellationTokenSource();
        public static CancellationTokenSource ctsStopDownloading = new CancellationTokenSource();

        public static void TaskBar()
        {
            notifyIcon.Visible = true;
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            notifyIcon.Text = "GTA Mission Downloader";
            notifyIcon.Click += (sender, e) => NotifyIconBalloonTipClicked();
        }
        public static async void NotifyIconBalloonTipClicked(bool stopOnStart = true, bool updateFiles = true)
        {
            MainWindow win = (MainWindow)System.Windows.Application.Current.MainWindow;
            if (stopOnStart) win.StopOnStart();
            win.WindowState = WindowState.Normal;
            win.Show();
            if (updateFiles) await CheckForUpdate.UpdateAsync(ctsOnStart.Token);
        }
        public static FilesResource.GetRequest GetFileRequest (string fileID, string field)
        {
            var request = service.Files.Get(fileID);
            request.Fields = $"{field}";

            return request;
        }
        public static void ButtonsOption(string whichOption)
        {
            MainWindow win = (MainWindow)System.Windows.Application.Current.MainWindow;
            switch (whichOption)
            {
                case "beforeDownload":
                    win.progressBarDownload.Visibility = Visibility.Visible;
                    win.textblockDownload.Visibility = Visibility.Visible;

                    win.S1AltisButton.IsEnabled = false;
                    win.S2AltisButton.IsEnabled = false;

                    win.S3MaldenButton.IsEnabled = false;
                    win.S3TanoaButton.IsEnabled = false;

                    win.S1S2AltisButton.IsEnabled = false;
                    win.S3MaldenS3TanoaButton.IsEnabled = false;
                    win.AllFiles.IsEnabled = false;

                    win.StopDownloadName.Visibility = Visibility.Visible;
                    break;
                case "afterDownload":
                    win.progressBarDownload.Visibility = Visibility.Hidden;
                    win.textblockDownload.Visibility = Visibility.Hidden;

                    win.S1AltisButton.IsEnabled = true;
                    win.S2AltisButton.IsEnabled = true;

                    win.S3MaldenButton.IsEnabled = true;
                    win.S3TanoaButton.IsEnabled = true;

                    win.S1S2AltisButton.IsEnabled = true;
                    win.S3MaldenS3TanoaButton.IsEnabled = true;
                    win.AllFiles.IsEnabled = true;

                    win.textblockDownload.Text = "";
                    win.progressBarDownload.Value = 0;
                    win.StopDownloadName.Visibility = Visibility.Hidden;
                    break;
                case "optionsCheckBoxOff":
                    win.S1AltisCheckBox.IsChecked = false;
                    win.S2AltisCheckBox.IsChecked = false;
                    win.S3MaldenCheckBox.IsChecked = false;
                    win.S3TanoaCheckBox.IsChecked = false;
                    break;
                case "deleteChangesToRegistry":
                    win.StartUpCheckBox.IsChecked = false;
                    win.HiddenCheckBox.IsChecked = false;
                    win.RunTSAuto.IsChecked = false;
                    win.NotificationCheckBox.IsChecked = false;
                    win.AutomaticUpdateCheckBox.IsChecked = false;
                    break;
            }
        }
    }
}