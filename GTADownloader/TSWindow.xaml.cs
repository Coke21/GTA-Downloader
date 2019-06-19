using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GTADownloader
{
    public partial class TSWindow : Window
    {
        public TSWindow()
        {
            InitializeComponent();
        }
        //ListView
        private void AddFileMenuItemClick(object sender, RoutedEventArgs e) => ListView.MenuItemClick("addPathClick");
        private void CopyPathMenuClick(object sender, RoutedEventArgs e) => ListView.MenuItemClick("copyPathClick");
        private void DeleteMenuClick(object sender, RoutedEventArgs e) => ListView.MenuItemClick("deletePathClick");

        private void LvItemMouseEnter(object sender, MouseEventArgs e) => ListView.LvItemMouseEnter(sender, e);
        private void LvMouseMoveDragDrop(object sender, MouseEventArgs e) => ListView.LvMouseMoveDragDrop(sender, e);
        private void TbChannelNameMouseMove(object sender, MouseEventArgs e) => ListView.TbChannelNameMouseMove(sender, e);

        private void PathDropTbCP(object sender, DragEventArgs e) => ListView.PathDropTbCP(sender, e);
        private void PathDropTbPass(object sender, DragEventArgs e) => ListView.PathDropTbPass(sender, e);
        private void PathDropLv(object sender, DragEventArgs e) => ListView.PathDropLv(sender, e);

        private void LvItemHotKeys(object sender, KeyEventArgs e) => ListView.LvItemHotkeys(sender, e);

        //End
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;
    }
}
