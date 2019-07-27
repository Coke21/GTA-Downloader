using System.Windows;
using System.Windows.Input;

namespace GTADownloader
{
    public partial class TSWindow : Window
    {
        public TSWindow()
        {
            InitializeComponent();
        }
        //ListView
        private void AddFileMenuItemClick(object sender, RoutedEventArgs e) => ListViewClass.MenuItemClick("addPathClick");
        private void CopyPathMenuClick(object sender, RoutedEventArgs e) => ListViewClass.MenuItemClick("copyPathClick");
        private void DeleteMenuClick(object sender, RoutedEventArgs e) => ListViewClass.MenuItemClick("deletePathClick");

        private void LvItemMouseEnter(object sender, MouseEventArgs e) => ListViewClass.LvItemMouseEnter(sender, e);
        private void LvMouseMoveDragDrop(object sender, MouseEventArgs e) => ListViewClass.LvMouseMoveDragDrop(sender, e);
        private void TbChannelNameMouseMove(object sender, MouseEventArgs e) => ListViewClass.TbChannelNameMouseMove(sender, e);

        private void PathDropTbCP(object sender, DragEventArgs e) => ListViewClass.PathDropTbCp(sender, e);
        private void PathDropTbPass(object sender, DragEventArgs e) => ListViewClass.PathDropTbPass(sender, e);
        private void PathDropLv(object sender, DragEventArgs e) => ListViewClass.PathDropLv(sender, e);

        private void LvItemHotKeys(object sender, KeyEventArgs e) => ListViewClass.LvItemHotkeys(sender, e);
        //End
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;
    }
}
