using System.Windows;
using System.Diagnostics;
using System;
using System.IO;
using System.Windows.Input;
using System.Threading;

namespace GTADownloader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FileData.Data();
            ServerJoinAsync();
            CallToCheckForUpdate();
            RemoveRemains();
            CheckForUpdate.TaskBar();
            Options.UpdateCheckBoxes();
        }
        private async void S1Altis(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[0], FileData.fileNameArray[0]);
        }
        private async void S2Altis(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[1], FileData.fileNameArray[1]);
        }
        private async void S3Altis(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[2], FileData.fileNameArray[2]);
        }
        private async void S1Tanoa(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[3], FileData.fileNameArray[3]);
        }
        private async void S2Tanoa(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[4], FileData.fileNameArray[4]);
        }
        private async void S3Tanoa(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[5], FileData.fileNameArray[5]);
        }
        private async void S1S2S3Altis(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[0], FileData.fileNameArray[0]);
            await Download.FileAsync(FileData.fileIDArray[1], FileData.fileNameArray[1]);
            await Download.FileAsync(FileData.fileIDArray[2], FileData.fileNameArray[2]);
        }
        private async void S1S2S3Tanoa(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[3], FileData.fileNameArray[3]);
            await Download.FileAsync(FileData.fileIDArray[4], FileData.fileNameArray[4]);
            await Download.FileAsync(FileData.fileIDArray[5], FileData.fileNameArray[5]);
        }
        private async void AllFiles(object sender, RoutedEventArgs e)
        {
            await Download.FileAsync(FileData.fileIDArray[0], FileData.fileNameArray[0]);
            await Download.FileAsync(FileData.fileIDArray[1], FileData.fileNameArray[1]);
            await Download.FileAsync(FileData.fileIDArray[2], FileData.fileNameArray[2]);
            await Download.FileAsync(FileData.fileIDArray[3], FileData.fileNameArray[3]);
            await Download.FileAsync(FileData.fileIDArray[4], FileData.fileNameArray[4]);
            await Download.FileAsync(FileData.fileIDArray[5], FileData.fileNameArray[5]);
        }
        // Is Update Available
        private async void CallToCheckForUpdate()
        {
            await CheckForUpdate.Update("onStart", FileData.ctsOnStart.Token);
        }
        private void JoinServerTabItemClicked(object sender, MouseButtonEventArgs e)
        {
            StopOnStar();
        }
        private void OptionsTabItemClicked(object sender, MouseButtonEventArgs e)
        {
            StopOnStar();
        }
        private void StopOnStar()
        {
            FileData.ctsOnStart.Cancel();
            FileData.ctsOnStart.Dispose();
            FileData.ctsOnStart = new CancellationTokenSource();

            TextTopOperationNotice.Text = "";
            TextTopOperationProgramNotice.Text = "";
        }

        // Join server
        private void JoinS1(object sender, RoutedEventArgs e)
        {
            JoinServer.Server("joinS1");
        }
        private void JoinS2(object sender, RoutedEventArgs e)
        {
            JoinServer.Server("joinS2");
        }
        private void JoinS3(object sender, RoutedEventArgs e)
        {
            JoinServer.Server("joinS3(Conflict)");
        }
        private void JoinTS(object sender, RoutedEventArgs e)
        {
            JoinServer.Server("joinTS");
        }
        private async void ServerJoinAsync()
        {
            await JoinServer.UpdateServerAsync();
        }

        //Options
        private void OfficialThread_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://grandtheftarma.com/topic/116196-gta-mission-downloader/");
        }
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"This application was made by Coke. Current version {FileData.programVersion}. \n" +
                $"Do you want to help develop this application? " +
                $"If so, head to official thread on GTA's forum and post your suggestion. \n" +
                $"Thank you for using this application! - Coke",
                "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void StartUpCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Options.Choose("startUp");
        }
        private void StartUpCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("startUpUnCheck");
        }
        private void RunMinimizedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Options.Choose("runMinimized");
        }
        private void RunMinimizedCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("runMinimizedUnCheck");
        }
        private void S1AltisChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S1Altis");
            FileData.missionFileListName.Add(FileData.fileNameArray[0]);
            FileData.missionFileListID.Add(FileData.fileIDArray[0]);
        }
        private void S1Altis_UnChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S1AltisUnCheck");
            FileData.missionFileListName.Remove(FileData.fileNameArray[0]);
            FileData.missionFileListID.Remove(FileData.fileIDArray[0]);
        }
        private void S2AltisChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S2Altis");
            FileData.missionFileListName.Add(FileData.fileNameArray[1]);
            FileData.missionFileListID.Add(FileData.fileIDArray[1]);
        }
        private void S2Altis_UnChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S2AltisUnCheck");
            FileData.missionFileListName.Remove(FileData.fileNameArray[1]);
            FileData.missionFileListID.Remove(FileData.fileIDArray[1]);
        }
        private void S3AltisChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S3Altis");
            FileData.missionFileListName.Add(FileData.fileNameArray[2]);
            FileData.missionFileListID.Add(FileData.fileIDArray[2]);
        }
        private void S3Altis_UnChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S3AltisUnCheck");
            FileData.missionFileListName.Remove(FileData.fileNameArray[2]);
            FileData.missionFileListID.Remove(FileData.fileIDArray[2]);
        }
        private void S1TanoaChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S1Tanoa");
            FileData.missionFileListName.Add(FileData.fileNameArray[3]);
            FileData.missionFileListID.Add(FileData.fileIDArray[3]);
        }
        private void S1Tanoa_UnChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S1TanoaUnCheck");
            FileData.missionFileListName.Remove(FileData.fileNameArray[3]);
            FileData.missionFileListID.Remove(FileData.fileIDArray[3]);
        }
        private void S2TanoaChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S2Tanoa");
            FileData.missionFileListName.Add(FileData.fileNameArray[4]);
            FileData.missionFileListID.Add(FileData.fileIDArray[4]);
        }
        private void S2Tanoa_UnChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S2TanoaUnCheck");
            FileData.missionFileListName.Remove(FileData.fileNameArray[4]);
            FileData.missionFileListID.Remove(FileData.fileIDArray[4]);
        }
        private void S3TanoaChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S3Tanoa");
            FileData.missionFileListName.Add(FileData.fileNameArray[5]);
            FileData.missionFileListID.Add(FileData.fileIDArray[5]);
        }
        private void S3Tanoa_UnChecked(object sender, RoutedEventArgs e)
        {
            Options.Choose("S3TanoaUnCheck");
            FileData.missionFileListName.Remove(FileData.fileNameArray[5]);
            FileData.missionFileListID.Remove(FileData.fileIDArray[5]);
        }
        private async void NotificationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Options.Choose("notification");
            await Options.NotificationAsync("notification", FileData.ctsNotification.Token);
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
            await Options.NotificationAsync("automaticUpdate", FileData.ctsAutomaticUpdate.Token);
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
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        public void RemoveRemains()
        {
            foreach (var item in FileData.fileNameArray)
            {
                string filePath = FileData.getDownloadFolderPath + item;
                if (File.Exists(filePath))
                {
                    MessageBox.Show($"Found remains of {item}, deleting it...", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    File.Delete(filePath);
                }
            }
        }
    }
}
