using System.Windows;
using System.Diagnostics;
using System;
using System.IO;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace GTADownloader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void WindowLoad(object sender, RoutedEventArgs e)
        {
            Title = $"GTA Mission Downloader | {Data.programVersion} by Coke";
            FileOperation.IsFilePresent("config");
            ListView.PopulateLvOnStart();
            _ = CheckForUpdate.UpdateAsync(Data.ctsOnStart.Token);
            _ = Join.UpdateServerAsync();
            Data.TaskBar();
            Options.UpdateCheckBoxes();
        }
        // Mfs
        private async void S1Altis(object sender, RoutedEventArgs e) => await Download.FileAsync(Data.fileIDArray[0], Data.ctsStopDownloading.Token);
        private async void S2Altis(object sender, RoutedEventArgs e) => await Download.FileAsync(Data.fileIDArray[1], Data.ctsStopDownloading.Token);
        private async void S3Tanoa(object sender, RoutedEventArgs e) => await Download.FileAsync(Data.fileIDArray[2], Data.ctsStopDownloading.Token);
        private async void S3Malden(object sender, RoutedEventArgs e) => await Download.FileAsync(Data.fileIDArray[3], Data.ctsStopDownloading.Token);
        private async void S1S2Altis(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(Data.fileIDArray[0], Data.ctsStopDownloading.Token);
            await Download.FileAsync(Data.fileIDArray[1], Data.ctsStopDownloading.Token);
        }
        private async void S3MaldenS3Tanoa(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(Data.fileIDArray[2], Data.ctsStopDownloading.Token);
            await Download.FileAsync(Data.fileIDArray[3], Data.ctsStopDownloading.Token);
        }
        private async void AllFilesClick(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(Data.fileIDArray[0], Data.ctsStopDownloading.Token);
            await Download.FileAsync(Data.fileIDArray[1], Data.ctsStopDownloading.Token);
            await Download.FileAsync(Data.fileIDArray[2], Data.ctsStopDownloading.Token);
            await Download.FileAsync(Data.fileIDArray[3], Data.ctsStopDownloading.Token);
        }
        private void StopDownloadClick(object sender, RoutedEventArgs e)
        {
            Data.ctsStopDownloading.Cancel();
            Data.ctsStopDownloading.Dispose();
            Data.ctsStopDownloading = new CancellationTokenSource();
        }
        // Stop Notification
        private void JoinServerTabItemClicked(object sender, MouseButtonEventArgs e) => StopOnStart();
        private void OptionsTabItemClicked(object sender, MouseButtonEventArgs e) => StopOnStart();
        public void StopOnStart()
        {
            Data.ctsOnStart.Cancel();
            Data.ctsOnStart.Dispose();
            Data.ctsOnStart = new CancellationTokenSource();

            TextTopOperationNotice.Text = "";
            TextTopOperationProgramNotice.Text = "";
        }
        // Join server
        private void JoinS1(object sender, RoutedEventArgs e) => Join.Server("joinS1");
        private void JoinS2(object sender, RoutedEventArgs e) => Join.Server("joinS2");
        private void JoinS3(object sender, RoutedEventArgs e) => Join.Server("joinS3(Conflict)");
        private void JoinTS(object sender, RoutedEventArgs e) => Join.Server("joinTS", false);

        //ListView
        private void AddFileMenuItemClick(object sender, RoutedEventArgs e) => ListView.MenuItemClick("addPathClick");
        private void CopyPathMenuClick(object sender, RoutedEventArgs e) => ListView.MenuItemClick("copyPathClick");
        private void DeleteMenuClick(object sender, RoutedEventArgs e) => ListView.MenuItemClick("deletePathClick");

        private void LvItemMouseEnter(object sender, MouseEventArgs e) => ListView.LvItemMouseEnter(sender, e);
        private void LvMouseMoveDragDrop(object sender, MouseEventArgs e) => ListView.LvMouseMoveDragDrop(sender, e);
        private void TbMouseMoveDragDrop(object sender, MouseEventArgs e) => ListView.TbMouseMoveDragDrop(sender, e);

        private void PathDropTbCP(object sender, DragEventArgs e) => ListView.PathDropTbCP(sender, e);
        private void PathDropTbPass(object sender, DragEventArgs e) => ListView.PathDropTbPass(sender, e);
        private void PathDropLv(object sender, DragEventArgs e) => ListView.PathDropLv(sender, e);

        private void LvItemHotKeys(object sender, KeyEventArgs e) => ListView.LvItemHotkeys(sender, e);
        //Options
        private void OfficialThread_Click(object sender, RoutedEventArgs e) => Process.Start("https://grandtheftarma.com/topic/116196-gta-mission-downloader/");
        private void About_Click(object sender, RoutedEventArgs e) => MessageBox.Show($"Current version {Data.programVersion}. \n" +
                                                                    $"Do you want to help develop this application? " +
                                                                    $"If so, head to official thread on GTA's forum and post your suggestion. \n" +
                                                                    $"Thank you for using this application! - Coke",
                                                                    "Information", MessageBoxButton.OK, MessageBoxImage.Warning);

        private async void StartUpCheckBox_Checked(object sender, RoutedEventArgs e) => await Options.Choose("startUp");
        private async void StartUpCheckBox_Unchecked(object sender, RoutedEventArgs e) => await Options.Choose("startUpUnCheck");

        private async void RunHiddenCheckBox_Checked(object sender, RoutedEventArgs e) => await Options.Choose("runHidden");
        private async void RunHiddenCheckBox_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("runHiddenUnCheck");

        private async void RunTSAutoCheckBox_Checked(object sender, RoutedEventArgs e) => await Options.Choose("runTSAuto");
        private async void RunTSAutoCheckBox_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("runTSAutoUnCheck");

        private async void S1AltisChecked(object sender, RoutedEventArgs e) => await Options.Choose("S1Altis");
        private async void S1Altis_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("S1AltisUnCheck");

        private async void S2AltisChecked(object sender, RoutedEventArgs e) => await Options.Choose("S2Altis");
        private async void S2Altis_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("S2AltisUnCheck");

        private async void S3TanoaChecked(object sender, RoutedEventArgs e) => await Options.Choose("S3Tanoa");
        private async void S3Tanoa_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("S3TanoaUnCheck");

        private async void S3MaldenChecked(object sender, RoutedEventArgs e) => await Options.Choose("S3Malden");
        private async void S3Malden_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("S3MaldenUnCheck");

        private async void NotificationCheckBox_Checked(object sender, RoutedEventArgs e) => await Options.Choose("notification");
        private async void NotificationCheckBox_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("notificationUnCheck");

        private async void AutomaticUpdate_Checked(object sender, RoutedEventArgs e) => await Options.Choose("automaticUpdate");
        private async void AutomaticUpdateCheckBox_UnChecked(object sender, RoutedEventArgs e) => await Options.Choose("automaticUpdateUnCheck");

        private async void MaxSpeed_Checked(object sender, RoutedEventArgs e) => await Options.Choose("maxSpeed");
        private async void NormalSpeed_Checked(object sender, RoutedEventArgs e) => await Options.Choose("normalSpeed");

        private void OpenConfigFolder(object sender, RoutedEventArgs e) => Process.Start(Data.getProgramFolderPath);
        private async void DeleteChangesToRegistry(object sender, RoutedEventArgs e) => await Options.Choose("removeRegistry");

        protected override void OnClosed(EventArgs e)
        {
            var readingCP = File.ReadLines(Data.getConfigFilePath).Skip(4).Take(1).First();
            FileOperation.EditFileLine(readingCP, $"Default Lv channel={insertTSChannelName.Text}");

            var readingPass = File.ReadLines(Data.getConfigFilePath).Skip(5).Take(1).First();
            FileOperation.EditFileLine(readingPass, $"Default Lv password={ insertTSChannelPasswordName.Text}");

            Data.notifyIcon.Icon.Dispose();
            Data.notifyIcon.Dispose();

            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
