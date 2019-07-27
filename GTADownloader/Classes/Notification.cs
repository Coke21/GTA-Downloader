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
            Data.NotifyIcon.Click += (sender, e) => NotifyIconBalloonTipClicked();
        }
        public static async void NotifyIconBalloonTipClicked(bool stopOnStart = true, bool updateFiles = true)
        {
            if (stopOnStart) StopNotification();
            Win.WindowState = WindowState.Normal;
            Win.Show();
            if (updateFiles) await CheckForUpdate.UpdateAsync(Data.CtsOnStart.Token);
        }
        public static void StopNotification()
        {
            Data.CtsOnStart.Cancel();
            Data.CtsOnStart.Dispose();
            Data.CtsOnStart = new CancellationTokenSource();

            Win.TextTopOperationNotice.Text = "";
            Win.TextTopOperationProgramNotice.Text = "";

            Win.ProgramUpdateName.Visibility = Visibility.Hidden;
            Win.ReadChangelogName.Visibility = Visibility.Hidden;
        }
    }
}
