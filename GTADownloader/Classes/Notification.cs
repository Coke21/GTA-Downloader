using System.Threading;
using System.Windows;

namespace GTADownloader
{
    class Notification
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;

        public static void EnableTaskBar()
        {
            Data.notifyIcon.Visible = true;
            Data.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            Data.notifyIcon.Text = "GTA Mission Downloader";
            Data.notifyIcon.Click += (sender, e) => NotifyIconBalloonTipClicked();
        }
        public static async void NotifyIconBalloonTipClicked(bool stopOnStart = true, bool updateFiles = true)
        {
            if (stopOnStart) StopNotification();
            Win.WindowState = WindowState.Normal;
            Win.Show();
            if (updateFiles) await CheckForUpdate.UpdateAsync(Data.ctsOnStart.Token);
        }
        public static void StopNotification()
        {
            Data.ctsOnStart.Cancel();
            Data.ctsOnStart.Dispose();
            Data.ctsOnStart = new CancellationTokenSource();

            Win.TextTopOperationNotice.Text = "";
            Win.TextTopOperationProgramNotice.Text = "";

            Win.ProgramUpdateName.Visibility = Visibility.Hidden;
            Win.ReadChangelogName.Visibility = Visibility.Hidden;
        }
    }
}
