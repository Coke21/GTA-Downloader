using System.Threading;
using System.Windows.Forms;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace GTADownloader
{
    class Data
    {
        public const string ProgramVersion = "1.1";

        public const string FolderId = "0B8j-xMQtDZvwVjN6R25sWF94dG8";
        public const string ProgramId = "1EHQqd72EELxE-GXFCS4urWzn_3fL5wI2";

        public static NotifyIcon NotifyIcon = new NotifyIcon();

        public static CancellationTokenSource CtsOnStart = new CancellationTokenSource();
        public static CancellationTokenSource CtsAutomaticUpdate = new CancellationTokenSource();
        public static CancellationTokenSource CtsStopDownloading = new CancellationTokenSource();

        public static DriveService Service = new DriveService(new BaseClientService.Initializer()
        {
            ApiKey = "AIzaSyB8KixGHl2SPwQ5HJixKGm7IGbOYbpuc1w"
        });
        public static FilesResource.GetRequest GetFileRequest(string fileId, string field)
        {
            var request = Service.Files.Get(fileId);
            request.Fields = $"{field}";

            return request;
        }
    }
}