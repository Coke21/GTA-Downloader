using Microsoft.Win32;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GTADownloader
{
    class Options
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;
        public static async Task Choose (string whichOption)
        {
            RegistryKey keyStartUp = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            switch (whichOption)
            {
                case "startUp":
                    keyStartUp.SetValue("GTADownloader", System.Reflection.Assembly.GetExecutingAssembly().Location);
                    break;
                case "startUpUnCheck":
                    keyStartUp.DeleteValue("GTADownloader");
                    break;
                case "runHidden":
                    Win.StartUpCheckBox.IsChecked = true;
                    Win.StartUpCheckBox.IsEnabled = false;
                    break;
                case "runHiddenUnCheck":
                    Win.StartUpCheckBox.IsEnabled = true;
                    Win.ShowInTaskbar = true;
                    break;
                case "S1Altis":
                    Data.missionFileListID.Add(Data.fileIDArray[0]);
                    break;
                case "S1AltisUnCheck":
                    Data.missionFileListID.Remove(Data.fileIDArray[0]);
                    break;
                case "S2Altis":
                    Data.missionFileListID.Add(Data.fileIDArray[1]);
                    break;
                case "S2AltisUnCheck":
                    Data.missionFileListID.Remove(Data.fileIDArray[1]);
                    break;
                case "S3Tanoa":
                    Data.missionFileListID.Add(Data.fileIDArray[2]);
                    break;
                case "S3TanoaUnCheck":
                    Data.missionFileListID.Remove(Data.fileIDArray[2]);
                    break;
                case "S3Malden":
                    Data.missionFileListID.Add(Data.fileIDArray[3]);
                    break;
                case "S3MaldenUnCheck":
                    Data.missionFileListID.Remove(Data.fileIDArray[3]);
                    break;
                case "notification":
                    await CheckForUpdate.TypeOfNotificationAsync("notification", Data.ctsNotification.Token);
                    break;
                case "notificationUnCheck":
                    Data.ctsNotification.Cancel();
                    Data.ctsNotification.Dispose();
                    Data.ctsNotification = new CancellationTokenSource();

                    DataHelper.ButtonsOption("optionsCheckBoxOff");
                    break;
                case "automaticUpdate":
                    await CheckForUpdate.TypeOfNotificationAsync("automaticUpdate", Data.ctsAutomaticUpdate.Token);
                    break;
                case "automaticUpdateUnCheck":
                    Data.ctsAutomaticUpdate.Cancel();
                    Data.ctsAutomaticUpdate.Dispose();
                    Data.ctsAutomaticUpdate = new CancellationTokenSource();

                    DataHelper.ButtonsOption("optionsCheckBoxOff");
                    break;
                case "maxSpeed":
                    DataProperties.DownloadSpeed = "maxSpeed";
                    break;
                case "normalSpeed":
                    DataProperties.DownloadSpeed = "normalSpeed";
                    break;
            }
        }
    }
}
