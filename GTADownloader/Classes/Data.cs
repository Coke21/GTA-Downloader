using System.Collections.Generic;
using System.Threading;
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

        public static List<string> missionFileListID = new List<string>();
        public static NotifyIcon notifyIcon = new NotifyIcon();

        public static CancellationTokenSource ctsOnStart = new CancellationTokenSource();
        public static CancellationTokenSource ctsNotification = new CancellationTokenSource();
        public static CancellationTokenSource ctsAutomaticUpdate = new CancellationTokenSource();
        public static CancellationTokenSource ctsStopDownloading = new CancellationTokenSource();
    }
}