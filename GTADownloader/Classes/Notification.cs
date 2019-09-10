using System.IO;
using System.Threading;
using System.Windows;

namespace GTADownloader
{
    class Notification
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;

        public static void EnableTaskBar()
        {
            Data.NotifyIcon.Visible = true;
            Data.NotifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            Data.NotifyIcon.Text = "GTA Mission Downloader";
            Data.NotifyIcon.Click += (sender, e) => NotifyIconBalloonTipClicked(true, true);
            Data.NotifyIcon.BalloonTipClicked += (a, b) => NotifyIconBalloonTipClicked(false, false);
        }
        private static async void NotifyIconBalloonTipClicked(bool stopOnStart, bool areFilesUpdated)
        {
            if (stopOnStart) StopNotification();
            Win.WindowState = WindowState.Normal;
            Win.Show();
            try
            {
                if (areFilesUpdated)
                    await Update.FilesCheckAsync(Data.CtsOnStart.Token);
            }
            catch (IOException)
            {
            }
        }
        private static void StopNotification()
        {
            Data.CtsOnStart.Cancel();
            Data.CtsOnStart.Dispose();
            Data.CtsOnStart = new CancellationTokenSource();

            Win.TextTopOperationProgramNotice.Text = "";

            Win.ProgramUpdateName.Visibility = Visibility.Hidden;
            Win.ReadChangelogName.Visibility = Visibility.Hidden;
        }
    }
}
