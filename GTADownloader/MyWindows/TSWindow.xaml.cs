using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using GTADownloader.Classes;

namespace GTADownloader
{
    public partial class TSWindow
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;
        private ObservableCollection<TSWindowProperties.LvItem> Items { get; set; } = new ObservableCollection<TSWindowProperties.LvItem>();
        private TSWindowProperties.LvItem CurrentItem { get; set; }

        public TSWindow()
        {
            InitializeComponent();

            Persistence.Tracker.Configure<TSWindow>()
                .Id(p => p.Name)

                .Properties(p => new {p.Items})

                .Property(p => p.InsertTsChannelName.Text, string.Empty, "Default TS channel")
                .Property(p => p.InsertTsChannelPasswordName.Text, string.Empty, "Default TS password")

                .PersistOn(nameof(TeamSpeakWindow.MouseEnter), TeamSpeakWindow)
                .PersistOn(nameof(TeamSpeakWindow.MouseLeave), TeamSpeakWindow);

            Persistence.Tracker.Track(this);

            DataContext = Items;
        }
        //No window move
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        handled = true;
                    break;
            }
            return IntPtr.Zero;
        }
        //ListView
        private void AddFileMenuItemClick(object sender, RoutedEventArgs e) => MenuItemClick("addPathClick");
        private void CopyPathMenuClick(object sender, RoutedEventArgs e) => MenuItemClick("copyPathClick");
        private void DeleteMenuClick(object sender, RoutedEventArgs e) => MenuItemClick("deletePathClick");

        private void MenuItemClick(string menuItem)
        {
            TSWindowProperties.LvItem selectedItem = (TSWindowProperties.LvItem)LvName.SelectedItem;

            if (menuItem == "addPathClick")
            {
                if (Clipboard.GetText() == "") return;

                if (Items.All(item => item.ChannelPath != Clipboard.GetText()))
                {
                    Items.Add(new TSWindowProperties.LvItem { ChannelPath = $"{Clipboard.GetText()}" });
                    LvName.ItemsSource = Items;
                }
                else
                    MessageBox.Show($"The \"{Clipboard.GetText()}\" parameter is already in the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (selectedItem != null)
                {
                    if (menuItem == "copyPathClick")
                        Clipboard.SetDataObject(selectedItem.ChannelPath);
                    if (menuItem == "deletePathClick")
                        Items.Remove(selectedItem);
                }
                else
                    MessageBox.Show("The parameter wasn't selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Mouse Events
        private void LvName_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListViewItem))
                LvName.UnselectAll();
        }
        private void LvItemMouseEnter(object sender, MouseEventArgs e)
        {
            LvName.Focus();
            var item = sender as ListViewItem;
            CurrentItem = (TSWindowProperties.LvItem)item.Content;
        }
        //DragDrop
        private void LvMouseMoveDragDrop(object sender, MouseEventArgs e)
        {
            if (LvName.SelectedItems.Count <= 0) return;

            if (Mouse.LeftButton != MouseButtonState.Pressed) return;

            if (LvName.SelectedItem is TSWindowProperties.LvItem mySelectedItem)
                DragDrop.DoDragDrop(LvName, mySelectedItem.ChannelPath, DragDropEffects.Copy);
        }
        private void TbChannelNameMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;

            if (!InsertTsChannelName.IsMouseOver)
                DragDrop.DoDragDrop(InsertTsChannelName, InsertTsChannelName.Text, DragDropEffects.Copy);
        }
        // Channel path land
        private void PathDropTbCp(object sender, DragEventArgs e)
        {
            InsertTsChannelName.Clear();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                InsertTsChannelName.Text = (string)e.Data.GetData(DataFormats.FileDrop);
        }
        private void PathDropTbPass(object sender, DragEventArgs e)
        {
           InsertTsChannelPasswordName.Clear();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                InsertTsChannelPasswordName.Text = (string)e.Data.GetData(DataFormats.FileDrop);
        }
        //On drop on listview
        private void PathDropLv(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.Text)) return;

            Clipboard.SetDataObject((string)e.Data.GetData(DataFormats.Text) ?? throw new InvalidOperationException());
            MenuItemClick("addPathClick");
        }
        //HotKey
        private void LvItemHotKeys(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                Clipboard.SetDataObject(CurrentItem.ChannelPath);

            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                MenuItemClick("addPathClick");

            if (e.Key == Key.Delete)
                if (CurrentItem != null)
                    Items.Remove(CurrentItem);
        }
        //End
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Win.Expander_Collapsed(null, null);
            e.Cancel = true;
        } 
    }
}
