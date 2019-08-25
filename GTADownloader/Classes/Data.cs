using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace GTADownloader
{
    class Data
    {
        public const string ProgramVersion = "1.0f";

        public static readonly string[] FileIdArray = {"1Z7gMtm-KSL4wmoEcP3CaZKVLtjIKiv-8",
                                                       "1FDn0n5NVsHuiuIRysK96393OH_nUhU46",
                                                       "1ZJQBHLuMK3-OT-BRglVg83wE2jEMrZgD"};
        public static string[] FileNameArray = new string[3];

        public const string ProgramId = "1EHQqd72EELxE-GXFCS4urWzn_3fL5wI2";

        public static List<string> MissionFileListId = new List<string>();
        public static NotifyIcon NotifyIcon = new NotifyIcon();

        public static CancellationTokenSource CtsOnStart = new CancellationTokenSource();
        public static CancellationTokenSource CtsNotification = new CancellationTokenSource();
        public static CancellationTokenSource CtsAutomaticUpdate = new CancellationTokenSource();
        public static CancellationTokenSource CtsStopDownloading = new CancellationTokenSource();
    }
}