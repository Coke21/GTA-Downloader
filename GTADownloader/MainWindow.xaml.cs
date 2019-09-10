using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using GTADownloader.Classes;
using Microsoft.Win32;

namespace GTADownloader
{
    public partial class MainWindow
    {
        public ObservableCollection<ListViewProperties.ListViewItems> Items { get; set; } = new ObservableCollection<ListViewProperties.ListViewItems>();

        public MainWindow()
        {
            InitializeComponent();

            Persistence.Tracker.Configure<MainWindow>()
                .Id(p => p.Name)

                .Property(p => p.Items)

                .Property(p => p.StartUpCheckBox.IsChecked, false, "StartUp checkbox")
                .Property(p => p.HiddenCheckBox.IsChecked, false, "Hide at startup checkbox")
                .Property(p => p.RunTsAutoCheckBox.IsChecked, false, "Run TS automatically checkbox")
                .Property(p => p.AutomaticUpdateCheckBox.IsChecked, false, "Automatic update checkbox")

                .PersistOn(nameof(Closing));

            Persistence.Tracker.Track(this);

            DataContext = Items;
        }
        private async void WindowLoad(object sender, RoutedEventArgs e)
        {
            Title = $"GTA Mission Downloader | {Data.ProgramVersion} by Coke";
            DataProperties.W2.Owner = this;

            Directory.CreateDirectory(DataProperties.GetArma3FolderPath);
            Directory.CreateDirectory(DataProperties.GetArma3MissionFolderPath);

            if (Directory.Exists(DataProperties.GetProgramDataFolderPathOLD))
            {
                var location = new DirectoryInfo(DataProperties.GetProgramDataFolderPathOLD);
                location.Delete(true);
            }

            Notification.EnableTaskBar();

            if (HiddenCheckBox.IsChecked.Value)
            {
                Data.NotifyIcon.ShowBalloonTip(4000, "Reminder!", $"The program is running in the background!", System.Windows.Forms.ToolTipIcon.None);
                ShowInTaskbar = false;
                Hide();
            }
            if (RunTsAutoCheckBox.IsChecked.Value)
            {
                Process[] process = Process.GetProcessesByName("ts3client_win64");
                if (process.Length == 0) Join.Server("joinTS", false);
            }

            Join.UpdateServerAsync();
            await Update.FilesCheckAsync(Data.CtsOnStart.Token);
        }
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            DataProperties.W2.Left = Left + ActualWidth - 7;
            DataProperties.W2.Top = Top + 52;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) => Window_LocationChanged(sender, e);

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl)) return;

            if (TabControl.SelectedIndex != 0 && TabControl.SelectedIndex != 2) return;

            Expander1.IsExpanded = false;
            Expander_Collapsed(null, null);
        }

        private async void ProgramUpdateClick(object sender, RoutedEventArgs e) => await Download.FileAsync(Data.ProgramId, null, Data.CtsStopDownloading.Token, "programUpdate");
        private void ReadChangelogClick(object sender, RoutedEventArgs e) => Process.Start("https://docs.google.com/document/d/1HzbVqK26YLsJtSBC2XJ7s_VcQ9IWH9ZWy3LEGEDwrJk/edit");
        private void StopDownloadClick(object sender, RoutedEventArgs e)
        {
            Data.CtsStopDownloading.Cancel();
            Data.CtsStopDownloading.Dispose();
            Data.CtsStopDownloading = new CancellationTokenSource();
        }

        private async void ListView_Initialized(object sender, EventArgs e)
        {
            var listRequest = Data.Service.Files.List();
            listRequest.OrderBy = "name";
            listRequest.Fields = "files(id, name, modifiedTime)";
            listRequest.Q = $"'{Data.FolderId}' in parents";
            var files = await listRequest.ExecuteAsync();

            var itemToRemove = files.Files.Single(r => r.Name == "readme.txt");
            files.Files.Remove(itemToRemove);

            if (Items.Count > 0)
            {
                MissionFileCheckbox_Checked(null, null);

                foreach (var item in Items.Zip(files.Files, Tuple.Create).ToList())
                {
                    if (item.Item1.FileId == item.Item2.Id) continue;

                    bool isChecked = item.Item1.IsChecked;

                    Items.Remove(item.Item1);
                    if (File.Exists(DataProperties.GetArma3MissionFolderPath + item.Item1.Mission))
                        File.Delete(DataProperties.GetArma3MissionFolderPath + item.Item1.Mission);

                    string status = await Update.LvItemsCheckAsync(item.Item2.Name, item.Item2.Id);

                    Items.Add(new ListViewProperties.ListViewItems
                    {
                        Mission = item.Item2.Name,
                        IsMissionUpdated = status,
                        ModifiedTime = item.Item2.ModifiedTime.Value.ToString("dd.MM.yyyy HH:mm"),
                        IsModifiedTimeUpdated = status,
                        FileId = item.Item2.Id,
                        IsChecked = isChecked
                    });
                }
            }
            else
                foreach (var file in files.Files)
                {
                    string status = await Update.LvItemsCheckAsync(file.Name, file.Id);

                    Items.Add(new ListViewProperties.ListViewItems
                    {
                        Mission = file.Name,
                        IsMissionUpdated = status,
                        ModifiedTime = file.ModifiedTime.Value.ToString("dd.MM.yyyy HH:mm"),
                        IsModifiedTimeUpdated = status,
                        FileId = file.Id,
                        IsChecked = false
                    });
                }                
        }
        private void LvName_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => ListViewProperties.SelectedItems = LvName.SelectedItems.Cast<ListViewProperties.ListViewItems>().ToList();
        private void MissionFileCheckbox_Checked(object sender, RoutedEventArgs e) => ListViewProperties.SelectedCheckboxes = Items.Where(ps => ps.IsChecked);

        private async void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (AutomaticUpdateCheckBox.IsChecked.Value) return;
                
            foreach (var item in ListViewProperties.SelectedItems)
                await Download.FileAsync(item.FileId, item, Data.CtsStopDownloading.Token);
        }
        // Join tab
        private void JoinS1(object sender, RoutedEventArgs e) => Join.Server("joinS1");
        private void JoinS2(object sender, RoutedEventArgs e) => Join.Server("joinS2");
        private void JoinS3(object sender, RoutedEventArgs e) => Join.Server("joinS3(Conflict)");
        private void JoinTs(object sender, RoutedEventArgs e) => Join.Server("joinTS");

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            DataProperties.W2.WindowStartupLocation = WindowStartupLocation.Manual;
            DataProperties.W2.Left = Left + ActualWidth - 7;
            DataProperties.W2.Top = Top + 52;
            DataProperties.W2.Show();
        }
        private void Expander_Collapsed(object sender, RoutedEventArgs e) => DataProperties.W2.Hide();

        //Options
        private RegistryKey KeyStartUp { get; } = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        private void StartUpCheckBox_Checked(object sender, RoutedEventArgs e) => KeyStartUp.SetValue("GTADownloader", System.Reflection.Assembly.GetExecutingAssembly().Location);
        private void StartUpCheckBox_Unchecked(object sender, RoutedEventArgs e) => KeyStartUp.DeleteValue("GTADownloader");

        private void RunHiddenCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            StartUpCheckBox.IsChecked = true;
            StartUpCheckBox.IsEnabled = false;
        }

        private void RunHiddenCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            StartUpCheckBox.IsEnabled = true;
            ShowInTaskbar = true;
        }

        private async void AutomaticUpdateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                await Update.UpdateLvItemsCheckAsync(Data.CtsAutomaticUpdate.Token);
            }
            catch (IOException)
            {
                AutomaticUpdateCheckBox.IsChecked = false;
            }
        }
        private void AutomaticUpdateCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            Data.CtsAutomaticUpdate.Cancel();
            Data.CtsAutomaticUpdate.Dispose();
            Data.CtsAutomaticUpdate = new CancellationTokenSource();
        }

        private void OpenConfigFolder(object sender, RoutedEventArgs e) => Process.Start(DataProperties.GetProgramDataFolderPathNEW);

        private void OfficialThread_Click(object sender, RoutedEventArgs e) => Process.Start("https://grandtheftarma.com/topic/116196-gta-mission-downloader/");
        private void About_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Do you want to help develop this application? " +
                                                                                                   "If so, head to official thread on GTA's forum and post your suggestion.\n" +
                                                                                                   "Thank you for using this application! - Coke",
                                                                                            "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
        public void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Data.NotifyIcon.Icon != null)
            {
                Data.NotifyIcon.Icon.Dispose();
                Data.NotifyIcon.Dispose();
            }

            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
