using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GTADownloader
{
    class Consistency
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;
        public static async Task Load()
        {
            Directory.CreateDirectory(DataProperties.GetArma3FolderPath);
            Directory.CreateDirectory(DataProperties.GetArma3MissionFolderPath);

            Notification.EnableTaskBar();

            DataProperties.W2.InsertTsChannelName.Text = Properties.Settings.Default.DefaultLvChannel;
            DataProperties.W2.InsertTsChannelPasswordName.Text = Properties.Settings.Default.DefaultLvPassword;

            if (Properties.Settings.Default.RunAtStartUp) Win.StartUpCheckBox.IsChecked = true;
            if (Properties.Settings.Default.GTAUpdate) Win.GtaUpdateCheckBox.IsChecked = true;
            if (Properties.Settings.Default.RunHidden)
            {
                Win.HiddenCheckBox.IsChecked = true;
                Win.ShowInTaskbar = false;
                Win.Hide();
                Data.NotifyIcon.ShowBalloonTip(4000, "Reminder!", $"The program is running in the background!", System.Windows.Forms.ToolTipIcon.None);
                Data.NotifyIcon.BalloonTipClicked += (a, b) => Notification.NotifyIconBalloonTipClicked(false, false);
            }
            if (Properties.Settings.Default.RunTsAuto)
            {
                Win.RunTsAutoCheckBox.IsChecked = true;
                Process[] process = Process.GetProcessesByName("ts3client_win64");
                if (process.Length == 0) Join.Server("joinTS", false);
            }

            if (Properties.Settings.Default.S1Altis) Win.S1AltisCheckBox.IsChecked = true;
            if (Properties.Settings.Default.S2Altis) Win.S2AltisCheckBox.IsChecked = true;
            if (Properties.Settings.Default.S3Tanoa) Win.S3TanoaCheckBox.IsChecked = true;
            if (Properties.Settings.Default.S2Enoch) Win.S2EnochCheckBox.IsChecked = true;

            if (Properties.Settings.Default.Notification) Win.NotificationCheckBox.IsChecked = true;
            if (Properties.Settings.Default.AutomaticUpdate) Win.AutomaticUpdateCheckBox.IsChecked = true;

            switch (Properties.Settings.Default.DownloadSpeed)
            {
                case "1":
                    Win.MaxSpeedButton.IsChecked = true;
                    break;
                case "0":
                    Win.NormalSpeedButton.IsChecked = true;
                    break;
                default:
                    Win.MaxSpeedButton.IsChecked = true;
                    break;
            }

            if (Properties.Settings.Default.ListViewItems != null)
            {
                StringCollection collection = Properties.Settings.Default.ListViewItems;
                List<string> followedList = collection.Cast<string>().ToList();

                foreach (var item in followedList)
                {
                    ListViewClass.Items.Add(new ListViewClassProperties.LvItem() { ChannelPath = item });
                    DataProperties.W2.LvName.ItemsSource = ListViewClass.Items;
                }
            }

            _ = Join.UpdateServerAsync();
            await DataHelper.PopulateFileNameArray();
            await CheckForUpdate.UpdateAsync(Data.CtsOnStart.Token);
        }
        public static void Save()
        {
            Properties.Settings.Default.DefaultLvChannel = DataProperties.W2.InsertTsChannelName.Text;
            Properties.Settings.Default.DefaultLvPassword = DataProperties.W2.InsertTsChannelPasswordName.Text;

            Properties.Settings.Default.RunAtStartUp = Win.StartUpCheckBox.IsChecked.Value;
            Properties.Settings.Default.GTAUpdate = Win.GtaUpdateCheckBox.IsChecked.Value;
            Properties.Settings.Default.RunHidden = Win.HiddenCheckBox.IsChecked.Value;
            Properties.Settings.Default.RunTsAuto = Win.RunTsAutoCheckBox.IsChecked.Value;

            Properties.Settings.Default.S1Altis = Win.S1AltisCheckBox.IsChecked.Value;
            Properties.Settings.Default.S2Altis = Win.S2AltisCheckBox.IsChecked.Value;
            Properties.Settings.Default.S3Tanoa = Win.S3TanoaCheckBox.IsChecked.Value;
            Properties.Settings.Default.S2Enoch = Win.S2EnochCheckBox.IsChecked.Value;

            Properties.Settings.Default.Notification = Win.NotificationCheckBox.IsChecked.Value;
            Properties.Settings.Default.AutomaticUpdate = Win.AutomaticUpdateCheckBox.IsChecked.Value;

            string value = string.Empty;
            switch (DataProperties.DownloadSpeed)
            {
                case "maxSpeed":
                    value = "1";
                    break;
                case "normalSpeed":
                    value = "0";
                    break;
            }
            Properties.Settings.Default.DownloadSpeed = value;

            StringCollection collection = new StringCollection();
            foreach (var item in ListViewClass.Items)
            {
                collection.Add(item.ChannelPath);
            }
            Properties.Settings.Default.ListViewItems = collection;

            Properties.Settings.Default.Save();

            Data.NotifyIcon.Icon.Dispose();
            Data.NotifyIcon.Dispose();
        }
    }
}
