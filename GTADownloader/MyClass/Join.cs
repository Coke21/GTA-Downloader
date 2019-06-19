using System.Threading.Tasks;
using QueryMaster;
using QueryMaster.GameServer;
using System;
using System.Windows;
using System.Diagnostics;

namespace GTADownloader
{
    class Join
    {
        public static void Server(string server, bool showNotificationMsg = true)
        {
            switch (server)
            {
                case "joinS1":
                    string arg1 = "-connect=164.132.201.9:2302";
                    Process.Start(Data.getRegistryArma3EXEPath, arg1);
                    break;
                case "joinS2":
                    string arg2 = "-connect=164.132.201.12:2302";
                    Process.Start(Data.getRegistryArma3EXEPath, arg2);
                    break;
                case "joinS3(Conflict)":
                    string arg3 = "-connect=164.132.202.63:2302";
                    Process.Start(Data.getRegistryArma3EXEPath, arg3);
                    break;
                case "joinTS":
                    string arg4 = "TS.grandtheftarma.com:9987";
                    Process.Start("ts3server://" + arg4 + $"?channel={Data.W2.insertTSChannelName.Text}" + $"&channelpassword={Data.W2.insertTSChannelPasswordName.Text}");
                    break;
            }
            if (showNotificationMsg) MessageBox.Show("The application is starting...(this window can be closed)", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        public static async Task UpdateServerAsync()
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            await Task.Run(() =>
            {
                while (true)
                {
                    Server gta1 = ServerQuery.GetServerInstance(Game.Arma_3, "164.132.201.9", 2303, false, 1000, 1000, 0, true);
                    ServerInfo info1 = gta1.GetInfo();
                    win.Dispatcher.BeginInvoke((Action)(() => GetServerInfo(info1, win.textBlockServer1, win.JoinServer1Button)));

                    Server gta2 = ServerQuery.GetServerInstance(Game.Arma_3, "164.132.201.12", 2303, false, 1000, 1000, 0, false);
                    ServerInfo info2 = gta2.GetInfo();
                    win.Dispatcher.BeginInvoke((Action)(() => GetServerInfo(info2, win.textBlockServer2, win.JoinServer2Button)));

                    Server gta3 = ServerQuery.GetServerInstance(Game.Arma_3, "164.132.202.63", 2303, false, 1000, 1000, 0, false);
                    ServerInfo info3 = gta3.GetInfo();
                    win.Dispatcher.BeginInvoke((Action)(() => GetServerInfo(info3, win.textBlockServer3, win.JoinServer3Button)));

                    Task.Delay(2000);
                }
            });
        }
        private static void GetServerInfo(ServerInfo info, System.Windows.Controls.TextBlock textBlock, System.Windows.Controls.Button button)
        {
            if (info != null)
            {
                if (info.Players + 1 != info.MaxPlayers)
                {
                    textBlock.Text = $"{info.Name} Players: {info.Players + 1}/{info.MaxPlayers}";
                    button.IsEnabled = true;
                }
                else
                {
                    textBlock.Text = "Server full";
                    button.IsEnabled = false;
                }
            }
            else
            {
                textBlock.Text = "Server offline";
                button.IsEnabled = false;
            }
        }
    }
}
