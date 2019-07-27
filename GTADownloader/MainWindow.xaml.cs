using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Windows.Controls;

namespace GTADownloader
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_LocationChanged(object sender, System.EventArgs e)
        {
            DataProperties.W2.Left = Left + ActualWidth - 7;
            DataProperties.W2.Top = Top + 52;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) => Window_LocationChanged(sender, e);
        private async void WindowLoad(object sender, RoutedEventArgs e)
        {
            Title = $"GTA Mission Downloader | {Data.ProgramVersion} by Coke";
            DataProperties.W2.Owner = this;

            await Consistency.Load();
        }
        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                switch (TabControl.SelectedIndex)
                {
                    case 0:
                        Expander1.IsExpanded = false;
                        Expander_Collapsed(null, null);
                        break;
                    case 1:
                        Notification.StopNotification();
                        break;
                    case 2:
                        Notification.StopNotification();
                        Expander1.IsExpanded = false;
                        Expander_Collapsed(null, null);
                        break;
                }
            }
        }
        // Mfs
        private async void S1Altis(object sender, RoutedEventArgs e) => await Download.FileAsync(Data.FileIdArray[0], Data.CtsStopDownloading.Token);
        private async void S2Altis(object sender, RoutedEventArgs e) => await Download.FileAsync(Data.FileIdArray[1], Data.CtsStopDownloading.Token);
        private async void S3Tanoa(object sender, RoutedEventArgs e) => await Download.FileAsync(Data.FileIdArray[2], Data.CtsStopDownloading.Token);
        private async void S2Livonia(object sender, RoutedEventArgs e) => await Download.FileAsync(Data.FileIdArray[3], Data.CtsStopDownloading.Token);
        private async void S1S2Altis(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(Data.FileIdArray[0], Data.CtsStopDownloading.Token);
            await Download.FileAsync(Data.FileIdArray[1], Data.CtsStopDownloading.Token);
        }
        private async void S3MaldenS2Livonia(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(Data.FileIdArray[2], Data.CtsStopDownloading.Token);
            await Download.FileAsync(Data.FileIdArray[3], Data.CtsStopDownloading.Token);
        }
        private async void AllFilesClick(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(Data.FileIdArray[0], Data.CtsStopDownloading.Token);
            await Download.FileAsync(Data.FileIdArray[1], Data.CtsStopDownloading.Token);
            await Download.FileAsync(Data.FileIdArray[2], Data.CtsStopDownloading.Token);
            await Download.FileAsync(Data.FileIdArray[3], Data.CtsStopDownloading.Token);
        }
        private async void ProgramUpdateClick(object sender, RoutedEventArgs e) => await Download.FileAsync(Data.ProgramId, Data.CtsStopDownloading.Token, "programUpdate");
        private void ReadChangelogClick(object sender, RoutedEventArgs e) => Process.Start("https://docs.google.com/document/d/1HzbVqK26YLsJtSBC2XJ7s_VcQ9IWH9ZWy3LEGEDwrJk/edit");
        private void StopDownloadClick(object sender, RoutedEventArgs e)
        {
            Data.CtsStopDownloading.Cancel();
            Data.CtsStopDownloading.Dispose();
            Data.CtsStopDownloading = new CancellationTokenSource();
        }
        // Join server
        private void JoinS1(object sender, RoutedEventArgs e) => Join.Server("joinS1");
        private void JoinS2(object sender, RoutedEventArgs e) => Join.Server("joinS2");
        private void JoinS3(object sender, RoutedEventArgs e) => Join.Server("joinS3(Conflict)");
        private void JoinTs(object sender, RoutedEventArgs e) => Join.Server("joinTS", false);

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            DataProperties.W2.WindowStartupLocation = WindowStartupLocation.Manual;
            DataProperties.W2.Left = Left + ActualWidth - 7;
            DataProperties.W2.Top = Top + 52;
            DataProperties.W2.Show();
        }
        private void Expander_Collapsed(object sender, RoutedEventArgs e) => DataProperties.W2.Hide();
        //Options
        private async void StartUpCheckBox_Checked(object sender, RoutedEventArgs e) => await Options.Choose("startUp");
        private async void StartUpCheckBox_Unchecked(object sender, RoutedEventArgs e) => await Options.Choose("startUpUnCheck");

        private async void RunHiddenCheckBox_Checked(object sender, RoutedEventArgs e) => await Options.Choose("runHidden");
        private async void RunHiddenCheckBox_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("runHiddenUnCheck");

        private async void S1Altis_Checked(object sender, RoutedEventArgs e) => await Options.Choose("S1Altis");
        private async void S1Altis_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("S1AltisUnCheck");

        private async void S2Altis_Checked(object sender, RoutedEventArgs e) => await Options.Choose("S2Altis");
        private async void S2Altis_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("S2AltisUnCheck");

        private async void S3Tanoa_Checked(object sender, RoutedEventArgs e) => await Options.Choose("S3Tanoa");
        private async void S3Tanoa_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("S3TanoaUnCheck");

        private async void S2Livonia_Checked(object sender, RoutedEventArgs e) => await Options.Choose("S2Livonia");
        private async void S2Livonia_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("S2LivoniaUnCheck");

        private async void NotificationCheckBox_Checked(object sender, RoutedEventArgs e) => await Options.Choose("notification");
        private async void NotificationCheckBox_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("notificationUnCheck");

        private async void AutomaticUpdate_Checked(object sender, RoutedEventArgs e) => await Options.Choose("automaticUpdate");
        private async void AutomaticUpdateCheckBox_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("automaticUpdateUnCheck");

        private async void MaxSpeed_Checked(object sender, RoutedEventArgs e) => await Options.Choose("maxSpeed");
        private async void NormalSpeed_Checked(object sender, RoutedEventArgs e) => await Options.Choose("normalSpeed");

        private void OpenConfigFolder(object sender, RoutedEventArgs e) => Process.Start(DataProperties.GetProgramDataFolderPath);

        private void OfficialThread_Click(object sender, RoutedEventArgs e) => Process.Start("https://grandtheftarma.com/topic/116196-gta-mission-downloader/");
        private void About_Click(object sender, RoutedEventArgs e) => MessageBox.Show($"Do you want to help develop this application? " +
                                                                    $"If so, head to official thread on GTA's forum and post your suggestion.\n" +
                                                                    $"Thank you for using this application! - Coke",
                                                                    "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            Consistency.Save();

            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
