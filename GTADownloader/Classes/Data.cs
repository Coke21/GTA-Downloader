using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace GTADownloader
{
    class Data
    {
        public const string ProgramVersion = "1.0f";

        public static readonly string[] FileIdArray = {"1KIzqR9NMBZoxcdibMZPxr13__azGdGye",
                                                       "15Or16ZcPqSzGF6b41p7_IDGzI3SDGCnJ",
                                                       "1ZJQBHLuMK3-OT-BRglVg83wE2jEMrZgD",
                                                       "1Lfo4EPaHLZttlAI5EbfC5NV-5N2MFLvn"};
        public static string[] FileNameArray = new string[4];

        public const string ProgramId = "1EHQqd72EELxE-GXFCS4urWzn_3fL5wI2";

        public static List<string> MissionFileListId = new List<string>();
        public static NotifyIcon NotifyIcon = new NotifyIcon();

        public static CancellationTokenSource CtsOnStart = new CancellationTokenSource();
        public static CancellationTokenSource CtsNotification = new CancellationTokenSource();
        public static CancellationTokenSource CtsAutomaticUpdate = new CancellationTokenSource();
        public static CancellationTokenSource CtsStopDownloading = new CancellationTokenSource();
    }
}