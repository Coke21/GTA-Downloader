using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GTADownloader
{
    class ListView
    {
        private class LvItem
        {
            public string ChannelPath { get; set; }
        }
        private static LvItem CurrentItem { get; set; }
        private static List<LvItem> Items { get; set; } = new List<LvItem>();

        private static MainWindow win = (MainWindow)Application.Current.MainWindow;
        public static void MenuItemClick (string menuItem)
        {
            LvItem selectedItem = (LvItem)Data.W2.LvName.SelectedItem;
            switch (menuItem)
            {
                case "addPathClick":
                    if (Clipboard.GetText() == "") return;

                    if (!Items.Any(item => item.ChannelPath == Clipboard.GetText()))
                    {
                        if (Clipboard.GetText().Length > 0)
                        {
                            Items.Add(new LvItem() { ChannelPath = $"{Clipboard.GetText()}" });
                            Data.W2.LvName.ItemsSource = Items;

                            FileOperation.AppendFileLine($"{Clipboard.GetText()}");
                            Data.W2.LvName.Items.Refresh();
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
                    {
                        Items.Remove(selectedItem);
                        FileOperation.DeleteFileLine($"{selectedItem.ChannelPath}");
                        Data.W2.LvName.Items.Refresh();
                    }
                    else
                        MessageBox.Show("The channel path wasn't selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
        public static void PopulateLvOnStart()
        {
            if (File.Exists(Data.getListViewFilePath))
            {
                foreach (var item in File.ReadLines(Data.getListViewFilePath).Skip(4))
                {
                    Items.Add(new LvItem() { ChannelPath = $"{item}" });
                    Data.W2.LvName.ItemsSource = Items;
                }
            }
        }
        //Mouse Events
        public static void LvItemMouseEnter(object sender, MouseEventArgs e)
        {
            Data.W2.LvName.Focus();
            var item = sender as ListViewItem;
            CurrentItem = (LvItem)item.Content;
        }
        //DragDrop
        public static void LvMouseMoveDragDrop(object sender, MouseEventArgs e)
        {
            if (Data.W2.LvName.SelectedItems.Count > 0)
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                    if (Data.W2.LvName.SelectedItem is LvItem mySelectedItem)
                        DragDrop.DoDragDrop(Data.W2.LvName, mySelectedItem.ChannelPath, DragDropEffects.Copy);
        }
        public static void TbChannelNameMouseMove(object sender, MouseEventArgs e)
        {
            if(Mouse.LeftButton == MouseButtonState.Pressed)
                if(!Data.W2.insertTSChannelName.IsMouseOver)
                    DragDrop.DoDragDrop(Data.W2.insertTSChannelName, Data.W2.insertTSChannelName.Text, DragDropEffects.Copy);
        }
        // Channel path land
        public static void PathDropTbCP(object sender, DragEventArgs e)
        {
            Data.W2.insertTSChannelName.Clear();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                Data.W2.insertTSChannelName.Text = (string)e.Data.GetData(DataFormats.FileDrop);
        }
        public static void PathDropTbPass(object sender, DragEventArgs e)
        {
            Data.W2.insertTSChannelPasswordName.Clear();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                Data.W2.insertTSChannelPasswordName.Text = (string)e.Data.GetData(DataFormats.FileDrop);
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
                    FileOperation.DeleteFileLine($"{CurrentItem.ChannelPath}");
                    Data.W2.LvName.Items.Refresh();
                }
            }
        }
    }
}
