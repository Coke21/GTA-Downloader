using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace GTADownloader
{
    class Data
    {
        public const string programVersion = "1.0d";

        public static readonly string[] fileIDArray = {"1KIzqR9NMBZoxcdibMZPxr13__azGdGye",
                                                        "15Or16ZcPqSzGF6b41p7_IDGzI3SDGCnJ",
                                                        "1ZJQBHLuMK3-OT-BRglVg83wE2jEMrZgD",
                                                        "1f5kNp5Erfs20J3u4pT2zBZNpV3A_f3sI"};
        public static string[] fileNameArray = new string[4];

        public const string programID = "1EHQqd72EELxE-GXFCS4urWzn_3fL5wI2";
        public static TSWindow W2 { get; set; } = new TSWindow();

        public static string getArma3FolderPath = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/Arma 3";
        public static string getArma3MissionFolderPath = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/Arma 3/MPMissionsCache/";

        public static string getProgramDataFolderPath = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/GTADownloader";
        public static string getConfigFilePath = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/GTADownloader/GTAData.txt";
        public static string getListViewFilePath = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/GTADownloader/GTALvData.txt";

        public static string getRegistryArma3EXEPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\bohemia interactive\arma 3", "main", string.Empty).ToString() + @"\arma3battleye";

        public static string getProgramFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\";
        public static string getProgramName = AppDomain.CurrentDomain.FriendlyName;

        public static List<string> missionFileListID = new List<string>();
        public static NotifyIcon notifyIcon = new NotifyIcon();

        public static CancellationTokenSource ctsOnStart = new CancellationTokenSource();
        public static CancellationTokenSource ctsNotification = new CancellationTokenSource();
        public static CancellationTokenSource ctsAutomaticUpdate = new CancellationTokenSource();
        public static CancellationTokenSource ctsStopDownloading = new CancellationTokenSource();
    }
}