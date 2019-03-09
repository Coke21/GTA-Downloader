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
            FileData.Data();
            RemoveRemainsAsync();
            OnStartAsync();
            ServerJoinAsync();
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
        private async void OnStartAsync() => await CheckForUpdate.Update(FileData.ctsOnStart.Token);
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
        private void JoinS1(object sender, RoutedEventArgs e) => JoinServer.Server("joinS1");
        private void JoinS2(object sender, RoutedEventArgs e) => JoinServer.Server("joinS2");
        private void JoinS3(object sender, RoutedEventArgs e) => JoinServer.Server("joinS3(Conflict)");
        private void JoinTS(object sender, RoutedEventArgs e) => JoinServer.Server("joinTS");
        private async void ServerJoinAsync() => await JoinServer.UpdateServerAsync();
        //Options
        private void OfficialThread_Click(object sender, RoutedEventArgs e) => Process.Start("https://grandtheftarma.com/topic/116196-gta-mission-downloader/");
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"This application was made by Coke. Current version {FileData.programVersion}. \n" +
                $"Do you want to help develop this application? " +
                $"If so, head to official thread on GTA's forum and post your suggestion. \n" +
                $"Thank you for using this application! - Coke",
                "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void StartUpCheckBox_Checked(object sender, RoutedEventArgs e) => Options.Choose("startUp");
        private void StartUpCheckBox_Unchecked(object sender, RoutedEventArgs e) => Options.Choose("startUpUnCheck");
        private void RunMinimizedCheckBox_Checked(object sender, RoutedEventArgs e) => Options.Choose("runMinimized");
        private void RunMinimizedCheckBox_UnChecked(object sender, RoutedEventArgs e) => Options.Choose("runMinimizedUnCheck");
        private void S1AltisChecked(object sender, RoutedEventArgs e)
        {
            FileData.missionFileListID.Add(FileData.fileIDArray[0]);
            Options.Choose("S1Altis");
        }
        private void S1Altis_UnChecked(object sender, RoutedEventArgs e)
        {
            FileData.missionFileListID.Remove(FileData.fileIDArray[0]);
            Options.Choose("S1AltisUnCheck");
        }
        private void S2AltisChecked(object sender, RoutedEventArgs e)
        {
            FileData.missionFileListID.Add(FileData.fileIDArray[1]);
            Options.Choose("S2Altis");
        }
        private void S2Altis_UnChecked(object sender, RoutedEventArgs e)
        {
            FileData.missionFileListID.Remove(FileData.fileIDArray[1]);
            Options.Choose("S2AltisUnCheck");
        }
        private void S3TanoaChecked(object sender, RoutedEventArgs e)
        {
            FileData.missionFileListID.Add(FileData.fileIDArray[2]);
            Options.Choose("S3Tanoa");
        }
        private void S3Tanoa_UnChecked(object sender, RoutedEventArgs e)
        {
            FileData.missionFileListID.Remove(FileData.fileIDArray[2]);
            Options.Choose("S3TanoaUnCheck");
        }
        private void S3MaldenChecked(object sender, RoutedEventArgs e)
        {
            FileData.missionFileListID.Add(FileData.fileIDArray[3]);
            Options.Choose("S3Malden");
        }
        private void S3Malden_UnChecked(object sender, RoutedEventArgs e)
        {
            FileData.missionFileListID.Remove(FileData.fileIDArray[3]);
            Options.Choose("S3MaldenUnCheck");
        }
        private async void NotificationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Options.Choose("notification");
            await Options.TypeOfNotification("notification", FileData.ctsNotification.Token);
        }
        private void NotificationCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            FileData.ctsNotification.Cancel();
            FileData.ctsNotification.Dispose();
            FileData.ctsNotification = new CancellationTokenSource();

            FileData.ButtonsOption("optionsCheckBoxOff");

            Options.Choose("notificationUnCheck");
        }
        private async void AutomaticUpdate_Checked(object sender, RoutedEventArgs e)
        {
            Options.Choose("automaticUpdate");
            await Options.TypeOfNotification("automaticUpdate", FileData.ctsAutomaticUpdate.Token);
        }
        private void AutomaticUpdateCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            FileData.ctsAutomaticUpdate.Cancel();
            FileData.ctsAutomaticUpdate.Dispose();
            FileData.ctsAutomaticUpdate = new CancellationTokenSource();

            FileData.ButtonsOption("optionsCheckBoxOff");

            Options.Choose("automaticUpdateUnCheck");
        }
        private void DeleteChangesToRegistry(object sender, RoutedEventArgs e)
        {
            FileData.ButtonsOption("optionsCheckBoxOff");
            FileData.ButtonsOption("deleteChangesToRegistry");

            Options.Choose("removeRegistry");
        }
        private void MaxSpeed_Checked(object sender, RoutedEventArgs e)
        {
            Download.downloadSpeed = "maxSpeed";

            Options.Choose("maxSpeed");
        }
        private void NormalSpeed_Checked(object sender, RoutedEventArgs e)
        {
            Download.downloadSpeed = "normalSpeed";

            Options.Choose("normalSpeed");
        }

        protected override void OnClosed(EventArgs e)
        {
            FileData.notifyIcon.Icon.Dispose();
            FileData.notifyIcon.Dispose();

            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        private async void RemoveRemainsAsync()
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
