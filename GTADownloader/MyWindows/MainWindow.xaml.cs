using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GTADownloader.Classes;
using GTADownloader.MyProperties;
using MahApps.Metro;
using Microsoft.Win32;

namespace GTADownloader
{
    public partial class MainWindow
    {
        public ObservableCollection<ListViewProperties.ListViewItems> Items { get; set; } = new ObservableCollection<ListViewProperties.ListViewItems>();
        public ObservableCollection<AccentsProperties.AccentsItems> AccentsItems { get; set; } = new ObservableCollection<AccentsProperties.AccentsItems>();

        public MainWindow()
        {
            InitializeComponent();

            Persistence.Tracker.Configure<MainWindow>()
                .Id(p => p.Name)

                .Property(p => p.Items)

                .Property(p => p.ThemeToggleSwitch.IsChecked, false, "Theme")
                .Property(p => p.AccentComboBox.SelectedIndex, 2, "Accent")

                .Property(p => p.StartUpCheckBox.IsChecked, false, "StartUp checkbox")
                .Property(p => p.HiddenCheckBox.IsChecked, false, "Hide at startup checkbox")
                .Property(p => p.RunTsAutoCheckBox.IsChecked, false, "Run TS automatically checkbox")
                .Property(p => p.AutomaticUpdateCheckBox.IsChecked, false, "Automatic update checkbox")

                .PersistOn(nameof(Closing));

            Persistence.Tracker.Track(this);

            DataContext = Items;
        }
        private async void Window_Load(object sender, RoutedEventArgs e)
        {
            Title = $"GTA Mission Downloader | {Data.ProgramVersion} by Coke";
            ChangeTheme(ThemeToggleSwitch.IsChecked.Value ? "BaseLight" : "BaseDark");

            DataProperties.W2.Owner = this;

            Directory.CreateDirectory(DataProperties.GetArma3FolderPath);
            Directory.CreateDirectory(DataProperties.GetArma3MissionFolderPath);

            foreach (var color in ThemeManager.Accents)
                AccentsItems.Add(new AccentsProperties.AccentsItems { ColorName = color.Name });

            AccentComboBox.ItemsSource = AccentsItems;

            Join.UpdateServerAsync();
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
                if (process.Length == 0) Join.Server("joinTS");
            }

            MissionFileCheckbox_Checked(null, null);
            await Update.FilesCheckAsync(Data.CtsOnStart.Token);
        }
        //Second window
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            DataProperties.W2.Left = Left + ActualWidth + 1;
            DataProperties.W2.Top = Top;
        }
        //Tab change
        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl)) return;

            if (TabControl.SelectedIndex != 0 && TabControl.SelectedIndex != 2) return;

            TsExpander.IsExpanded = false;
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

        private void LvName_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => ListViewProperties.SelectedItems = LvName.SelectedItems.Cast<ListViewProperties.ListViewItems>().ToList();
        private void LvName_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListViewItem))
                LvName.UnselectAll();
        }

        public void MissionFileCheckbox_Checked(object sender, RoutedEventArgs e) => ListViewProperties.SelectedCheckboxes = Items.Where(ps => ps.IsChecked);

        private void LvInfoButton_OnClick(object sender, RoutedEventArgs e) => MessageBox.Show("These are the current colors and the meaning behind them in the list:\n" +
                                                                                               "Green - You have the updated version of the mission file.\n" +
                                                                                               "Red - You have the outdated version of the mission file.\n" +
                                                                                               "Orange - You don't have the mission file on your PC.\n\n" +

                                                                                               "Subscription of the mission files:\n" +
                                                                                               "1.Choose the mission files that you want to observe.\n" +
                                                                                               "2.Tick them.\n" +
                                                                                               "3.Go to Options tab and tick the Automatic Update checkbox.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        private async void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (AutomaticUpdateCheckBox.IsChecked.Value)
            {
                MessageBox.Show("You cannot have the automatic update checkbox ticked! Untick the checkbox to manually download a file!", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
                
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
            DataProperties.W2.Left = Left + ActualWidth + 1;
            DataProperties.W2.Top = Top;
            DataProperties.W2.Show();
        }
        public void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            TsExpander.IsExpanded = false;
            DataProperties.W2.Hide();
        } 

        //Theme + Accent
        private void ThemeToggleSwitch_OnChecked(object sender, RoutedEventArgs e) => ChangeTheme("BaseLight");
        private void ThemeToggleSwitch_OnUnchecked(object sender, RoutedEventArgs e) => ChangeTheme("BaseDark");
        private void ChangeTheme(string theme)
        {
            switch (theme)
            {
                case "BaseLight":
                    TsExpander.SetValue(MahApps.Metro.Controls.GroupBoxHelper.HeaderForegroundProperty, Brushes.Black);
                    break;
                case "BaseDark":
                    TsExpander.SetValue(MahApps.Metro.Controls.GroupBoxHelper.HeaderForegroundProperty, Brushes.White);
                    break;
            }

            Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, appStyle.Item2, ThemeManager.GetAppTheme(theme));
        }
        private void Accent_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string colorName = ((sender as ComboBox).SelectedItem as AccentsProperties.AccentsItems).ColorName;

            Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(colorName), appStyle.Item1);
        }

        //General Options
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

        private void OpenConfigFolder(object sender, RoutedEventArgs e) => Process.Start(DataProperties.GetProgramDataFolderPath);
        private void OpenMissionFileFolder(object sender, RoutedEventArgs e) => Process.Start(DataProperties.GetArma3MissionFolderPath);
        private void OfficialThread_Click(object sender, RoutedEventArgs e) => Process.Start("https://grandtheftarma.com/topic/116196-gta-mission-downloader/");
        //End
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
