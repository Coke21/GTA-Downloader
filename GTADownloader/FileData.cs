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
    class FileData
    {
        public static string programVersion = "0.9";
        public static void Data()
        {
            MainWindow win = (MainWindow)System.Windows.Application.Current.MainWindow;
            win.Title = $"GTA Mission Downloader | {programVersion} by Coke";
            win.ResizeMode = ResizeMode.CanMinimize;
            win.TextTopTitle.Text = "GTA Mission Downloader";
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public static string[] fileIDArray = {"0B8j-xMQtDZvwMWVRVTVXVzdUalk",
                                              "1bb3gp6JNWHkhC4k3NUiXO02LTSylGiRQ",
                                              "1rZOxhmJSPt54x9nh86kGU0VGZeBEyLUX",
                                              "1lpq3Bj642W4-W9wfHncEXkQT9Jyp-MPx",
                                              "1cmufDsK_8ujpX72j3demhXfop0CvxTQA",
                                              "1qF8N1RdbEIQJPl_m347mFUPusEclXZP6"};

        public static string[] fileNameArray = {"s1.grandtheftarma.Altis.pbo",
                                                "s2.grandtheftarma.Altis.pbo",
                                                "s3.grandtheftarma.Altis.pbo",
                                                "s1.grandtheftarma.Tanoa.pbo",
                                                "s2.grandtheftarma.Tanoa.pbo",
                                                "s3.grandtheftarma.Tanoa.pbo"};

        public static string programOnlineID = "1EHQqd72EELxE-GXFCS4urWzn_3fL5wI2";

        public static DriveService service = new DriveService(new BaseClientService.Initializer()
        {
            ApiKey = "PLACE_YOUR_KEY_HERE",
            ApplicationName = "NAME_IT_W/E_You_WANT",
        });

        public static string folderPath = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/Arma 3/MPMissionsCache/";

        public static string getDownloadFolderPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString() + @"\";
        public static string getSteamFolderPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\bohemia interactive\arma 3", "main", String.Empty).ToString() + @"\arma3battleye";

        public static string programPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\";
        public static string programName = AppDomain.CurrentDomain.FriendlyName;

        public static List<string> missionFileListName = new List<string>();
        public static List<string> missionFileListID = new List<string>();
        public static NotifyIcon notifyIcon = new NotifyIcon();

        public static CancellationTokenSource ctsOnStart = new CancellationTokenSource();
        public static CancellationTokenSource ctsNotification = new CancellationTokenSource();
        public static CancellationTokenSource ctsAutomaticUpdate = new CancellationTokenSource();
        public static CancellationTokenSource ctsStopDownloading = new CancellationTokenSource();


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
                    win.S3AltisButton.IsEnabled = false;

                    win.S1S2S3AltisButton.IsEnabled = false;

                    win.S1TanoaButton.IsEnabled = false;
                    win.S2TanoaButton.IsEnabled = false;
                    win.S3TanoaButton.IsEnabled = false;

                    win.S1S2S3TanoaButton.IsEnabled = false;

                    win.AllFilesButton.IsEnabled = false;
                    break;
                case "afterDownload":
                    win.progressBarDownload.Visibility = Visibility.Hidden;
                    win.textblockDownload.Visibility = Visibility.Hidden;

                    win.S1AltisButton.IsEnabled = true;
                    win.S2AltisButton.IsEnabled = true;
                    win.S3AltisButton.IsEnabled = true;

                    win.S1S2S3AltisButton.IsEnabled = true;

                    win.S1TanoaButton.IsEnabled = true;
                    win.S2TanoaButton.IsEnabled = true;
                    win.S3TanoaButton.IsEnabled = true;

                    win.S1S2S3TanoaButton.IsEnabled = true;

                    win.AllFilesButton.IsEnabled = true;

                    win.textblockDownload.Text = "";
                    win.progressBarDownload.Value = 0;
                    break;
                case "optionsCheckBoxOff":
                    win.S1AltisCheckBox.IsChecked = false;
                    win.S2AltisCheckBox.IsChecked = false;
                    win.S3AltisCheckBox.IsChecked = false;
                    win.S1TanoaCheckBox.IsChecked = false;
                    win.S2TanoaCheckBox.IsChecked = false;
                    win.S3TanoaCheckBox.IsChecked = false;
                    break;
                case "deleteChangesToRegistry":
                    win.StartUpCheckBox.IsChecked = false;
                    win.MinimizedCheckBox.IsChecked = false;
                    win.NotificationCheckBox.IsChecked = false;
                    win.AutomaticUpdateCheckBox.IsChecked = false;
                    break;
            }
        }
    }
}