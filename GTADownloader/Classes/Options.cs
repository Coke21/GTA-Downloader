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
                    Data.MissionFileListId.Add(Data.FileIdArray[0]);
                    break;
                case "S1AltisUnCheck":
                    Data.MissionFileListId.Remove(Data.FileIdArray[0]);
                    break;
                case "S2Altis":
                    Data.MissionFileListId.Add(Data.FileIdArray[1]);
                    break;
                case "S2AltisUnCheck":
                    Data.MissionFileListId.Remove(Data.FileIdArray[1]);
                    break;
                case "S3Tanoa":
                    Data.MissionFileListId.Add(Data.FileIdArray[2]);
                    break;
                case "S3TanoaUnCheck":
                    Data.MissionFileListId.Remove(Data.FileIdArray[2]);
                    break;
                case "S2Livonia":
                    Data.MissionFileListId.Add(Data.FileIdArray[3]);
                    break;
                case "S2LivoniaUnCheck":
                    Data.MissionFileListId.Remove(Data.FileIdArray[3]);
                    break;
                case "notification":
                    await CheckForUpdate.TypeOfNotificationAsync("notification", Data.CtsNotification.Token);
                    break;
                case "notificationUnCheck":
                    Data.CtsNotification.Cancel();
                    Data.CtsNotification.Dispose();
                    Data.CtsNotification = new CancellationTokenSource();

                    DataHelper.ButtonsOption("optionsCheckBoxOff");
                    break;
                case "automaticUpdate":
                    await CheckForUpdate.TypeOfNotificationAsync("automaticUpdate", Data.CtsAutomaticUpdate.Token);
                    break;
                case "automaticUpdateUnCheck":
                    Data.CtsAutomaticUpdate.Cancel();
                    Data.CtsAutomaticUpdate.Dispose();
                    Data.CtsAutomaticUpdate = new CancellationTokenSource();

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
