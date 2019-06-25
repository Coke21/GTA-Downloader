using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GTADownloader
{
    class ListViewClass
    {
        private static ListViewClassProperties.LvItem CurrentItem { get; set; }
        public static ObservableCollection<ListViewClassProperties.LvItem> Items { get; set; } = new ObservableCollection<ListViewClassProperties.LvItem>();
        public static void MenuItemClick (string menuItem)
        {
            ListViewClassProperties.LvItem selectedItem = (ListViewClassProperties.LvItem)DataProperties.W2.LvName.SelectedItem;
            switch (menuItem)
            {
                case "addPathClick":
                    if (Clipboard.GetText() == "") return;

                    if (Clipboard.GetText().Count() > 0)
                        if (!Items.Any(item => item.ChannelPath == Clipboard.GetText()))
                        {
                            if (Clipboard.GetText().Length > 0)
                            {
                                Items.Add(new ListViewClassProperties.LvItem() { ChannelPath = $"{Clipboard.GetText()}" });
                                DataProperties.W2.LvName.ItemsSource = Items;
                            }
                        }
                        else
                            MessageBox.Show($"The '{Clipboard.GetText()}' path is already in the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case "copyPathClick":
                    if (selectedItem != null)
                        Clipboard.SetText(selectedItem.ChannelPath);
                    else
                        MessageBox.Show("The channel path wasn't selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case "deletePathClick":
                    if (selectedItem != null)
                        Items.Remove(selectedItem);
                    else
                        MessageBox.Show("The channel path wasn't selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
        //Mouse Events
        public static void LvItemMouseEnter(object sender, MouseEventArgs e)
        {
            DataProperties.W2.LvName.Focus();
            var item = sender as ListViewItem;
            CurrentItem = (ListViewClassProperties.LvItem)item.Content;
        }
        //DragDrop
        public static void LvMouseMoveDragDrop(object sender, MouseEventArgs e)
        {
            if (DataProperties.W2.LvName.SelectedItems.Count > 0)
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                    if (DataProperties.W2.LvName.SelectedItem is ListViewClassProperties.LvItem mySelectedItem)
                        DragDrop.DoDragDrop(DataProperties.W2.LvName, mySelectedItem.ChannelPath, DragDropEffects.Copy);
        }
        public static void TbChannelNameMouseMove(object sender, MouseEventArgs e)
        {
            if(Mouse.LeftButton == MouseButtonState.Pressed)
                if(!DataProperties.W2.insertTSChannelName.IsMouseOver)
                    DragDrop.DoDragDrop(DataProperties.W2.insertTSChannelName, DataProperties.W2.insertTSChannelName.Text, DragDropEffects.Copy);
        }
        // Channel path land
        public static void PathDropTbCP(object sender, DragEventArgs e)
        {
            DataProperties.W2.insertTSChannelName.Clear();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                DataProperties.W2.insertTSChannelName.Text = (string)e.Data.GetData(DataFormats.FileDrop);
        }
        public static void PathDropTbPass(object sender, DragEventArgs e)
        {
            DataProperties.W2.insertTSChannelPasswordName.Clear();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                DataProperties.W2.insertTSChannelPasswordName.Text = (string)e.Data.GetData(DataFormats.FileDrop);
        }
        //On drop on listview
        public static void PathDropLv(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                Clipboard.SetText((string)e.Data.GetData(DataFormats.Text));
                MenuItemClick("addPathClick");
            }
        }
        //HotKey
        public static void LvItemHotkeys (object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                Clipboard.SetText(CurrentItem.ChannelPath);

            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                MenuItemClick("addPathClick");

            if (e.Key == Key.Delete)
            {
                if (CurrentItem != null)
                {
                    Items.Remove(CurrentItem);
                    DataProperties.W2.LvName.Items.Refresh();
                }
            }
        }
    }
}
