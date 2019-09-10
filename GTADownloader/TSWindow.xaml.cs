using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GTADownloader.Classes;

namespace GTADownloader
{
    public partial class TSWindow
    {
        private ObservableCollection<TSWindowProperties.LvItem> Items { get; set; } = new ObservableCollection<TSWindowProperties.LvItem>();
        private TSWindowProperties.LvItem CurrentItem { get; set; }

        public TSWindow()
        {
            InitializeComponent();

            Persistence.Tracker.Configure<TSWindow>()
                .Id(p => p.Name)

                .Properties(p => new {p.Items})
                .PersistOn(nameof(LvName.PreviewDrop), LvName)
                .PersistOn(nameof(LvName.PreviewKeyDown), LvName)
                .PersistOn(nameof(LvName.MouseEnter), LvName)
                .PersistOn(nameof(LvName.MouseLeave), LvName)

                .Property(p => p.InsertTsChannelName.Text, string.Empty, "Default TS channel")
                .PersistOn(nameof(InsertTsChannelName.TextChanged), InsertTsChannelName)

                .Property(p => p.InsertTsChannelPasswordName.Text, string.Empty, "Default TS password")
                .PersistOn(nameof(InsertTsChannelPasswordName.TextChanged), InsertTsChannelPasswordName);

            Persistence.Tracker.Track(this);

            LvName.ItemsSource = Items;
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
                    Items.Add(new TSWindowProperties.LvItem() { ChannelPath = $"{Clipboard.GetText()}" });
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
                        Clipboard.SetText(selectedItem.ChannelPath);
                    if (menuItem == "deletePathClick")
                        Items.Remove(selectedItem);
                }
                else
                    MessageBox.Show("The parameter wasn't selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Mouse Events
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

            Clipboard.SetText((string)e.Data.GetData(DataFormats.Text) ?? throw new InvalidOperationException());
            MenuItemClick("addPathClick");
        }
        //HotKey
        private void LvItemHotKeys(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                Clipboard.SetText(CurrentItem.ChannelPath);

            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                MenuItemClick("addPathClick");

            if (e.Key == Key.Delete)
                if (CurrentItem != null)
                    Items.Remove(CurrentItem);
        }
        //End
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;
    }
}
