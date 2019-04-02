using System.Windows;
using System.Diagnostics;
using System;
using System.IO;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;

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
            Title = $"GTA Mission Downloader | {FileData.programVersion} by Coke";
            RemoveRemainsAsync();
            CheckForUpdate.UpdateAsync(FileData.ctsOnStart.Token);
            Join.UpdateServerAsync();
            CheckForUpdate.TaskBar();
            Options.UpdateCheckBoxes();
        }
        private async void S1Altis(object sender, RoutedEventArgs e) => await Download.FileAsync(FileData.fileIDArray[0]);
        private async void S2Altis(object sender, RoutedEventArgs e) => await Download.FileAsync(FileData.fileIDArray[1]);
        private async void S3Tanoa(object sender, RoutedEventArgs e) => await Download.FileAsync(FileData.fileIDArray[2]);
        private async void S3Malden(object sender, RoutedEventArgs e) => await Download.FileAsync(FileData.fileIDArray[3]);
        private async void S1S2Altis(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[0]);
            await Download.FileAsync(FileData.fileIDArray[1]);
        }
        private async void S3MaldenS3Tanoa(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[2]);
            await Download.FileAsync(FileData.fileIDArray[3]);
        }
        private async void AllFilesClick(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[0]);
            await Download.FileAsync(FileData.fileIDArray[1]);
            await Download.FileAsync(FileData.fileIDArray[2]);
            await Download.FileAsync(FileData.fileIDArray[3]);
        }
        // Is Update Available
        private void JoinServerTabItemClicked(object sender, MouseButtonEventArgs e) => StopOnStart();
        private void OptionsTabItemClicked(object sender, MouseButtonEventArgs e) => StopOnStart();
        public void StopOnStart()
        {
            FileData.ctsOnStart.Cancel();
            FileData.ctsOnStart.Dispose();
            FileData.ctsOnStart = new CancellationTokenSource();

            TextTopOperationNotice.Text = "";
            TextTopOperationProgramNotice.Text = "";
        }
        // Join server
        private void JoinS1(object sender, RoutedEventArgs e) => Join.Server("joinS1");
        private void JoinS2(object sender, RoutedEventArgs e) => Join.Server("joinS2");
        private void JoinS3(object sender, RoutedEventArgs e) => Join.Server("joinS3(Conflict)");
        private void JoinTS(object sender, RoutedEventArgs e) => Join.Server("joinTS");
        //Options
        private void OfficialThread_Click(object sender, RoutedEventArgs e) => Process.Start("https://grandtheftarma.com/topic/116196-gta-mission-downloader/");
        private void Info_Click(object sender, RoutedEventArgs e) => MessageBox.Show($"Current version {FileData.programVersion}. \n" +
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

        private async void DeleteChangesToRegistry(object sender, RoutedEventArgs e) => await Options.Choose("removeRegistry");

        protected override void OnClosed(EventArgs e)
        {
            FileData.notifyIcon.Icon.Dispose();
            FileData.notifyIcon.Dispose();

            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        private async Task RemoveRemainsAsync()
        {
            await Task.Run(() =>
            {
                foreach (var item in FileData.fileIDArray)
                {
                    var request = FileData.service.Files.Get(item);
                    request.Fields = "name";
                    string fileName = request.Execute().Name;

                    string filePath = FileData.getDownloadFolderPath + fileName;
                    if (File.Exists(filePath))
                    {
                        MessageBox.Show($"Found remains of {fileName}, deleting it...", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        File.Delete(filePath);
                    }
                }
            });
        }
    }
}
