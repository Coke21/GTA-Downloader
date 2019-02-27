using System.Threading.Tasks;
using QueryMaster;
using QueryMaster.GameServer;
using System;
using System.Windows;
using System.Diagnostics;

namespace GTADownloader
{
    class JoinServer
    {
        public static void Server(string server)
        {
            MessageBox.Show("The application is starting...(this window can be closed)", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            switch (server)
            {
                case "joinS1":
                    string arg1 = "-connect=164.132.201.9:2302";
                    Process.Start(FileData.getSteamFolderPath, arg1);
                    break;
                case "joinS2":
                    string arg2 = "-connect=164.132.201.12:2302";
                    Process.Start(FileData.getSteamFolderPath, arg2);
                    break;
                case "joinS3(Conflict)":
                    string arg3 = "-connect=164.132.202.63:2302";
                    Process.Start(FileData.getSteamFolderPath, arg3);
                    break;
                case "joinTS":
                    string arg4 = "TS.grandtheftarma.com:9987";
                    Process.Start("ts3server://" + arg4);
                    break;
            }
        }
        public static async Task UpdateServerAsync()
        {
            MainWindow win = (MainWindow)Application.Current.MainWindow;
            await Task.Run(() =>
            {
                while (true)
                {
                    Server gta1 = ServerQuery.GetServerInstance(Game.Arma_3, "164.132.201.9", 2303, false, 1000, 1000, 0, false);
                    ServerInfo info1 = gta1.GetInfo();
                    if (info1 != null)
                    {
                        if (info1.Players + 1 != info1.MaxPlayers)
                        {
                            win.Dispatcher.BeginInvoke((Action)(() => win.textBlockServer1.Text = $"{info1.Name} Players: {info1.Players + 1}/{info1.MaxPlayers}"));
                            win.Dispatcher.BeginInvoke((Action)(() => win.JoinServer1Button.IsEnabled = true));
                        }
                        else
                        {
                            win.Dispatcher.BeginInvoke((Action)(() => win.textBlockServer1.Text = "Server full"));
                            win.Dispatcher.BeginInvoke((Action)(() => win.JoinServer1Button.IsEnabled = false));
                        }
                    }
                    else
                    {
                        win.Dispatcher.BeginInvoke((Action)(() => win.textBlockServer1.Text = "Server offline"));
                        win.Dispatcher.BeginInvoke((Action)(() => win.JoinServer1Button.IsEnabled = false));
                    }

                    Server gta2 = ServerQuery.GetServerInstance(Game.Arma_3, "164.132.201.12", 2303, false, 1000, 1000, 0, false);
                    ServerInfo info2 = gta2.GetInfo();
                    if (info2 != null)
                    {
                        if (info2.Players + 1 != info2.MaxPlayers)
                        {
                            win.Dispatcher.BeginInvoke((Action)(() => win.textBlockServer2.Text = $"{info2.Name} Players: {info2.Players + 1}/{info2.MaxPlayers}"));
                            win.Dispatcher.BeginInvoke((Action)(() => win.JoinServer2Button.IsEnabled = true));
                        }
                        else
                        {
                            win.Dispatcher.BeginInvoke((Action)(() => win.textBlockServer2.Text = "Server full"));
                            win.Dispatcher.BeginInvoke((Action)(() => win.JoinServer2Button.IsEnabled = false));
                        }
                    }
                    else
                    {
                        win.Dispatcher.BeginInvoke((Action)(() => win.textBlockServer2.Text = "Server offline"));
                        win.Dispatcher.BeginInvoke((Action)(() => win.JoinServer2Button.IsEnabled = false));
                    }

                    Server gta3 = ServerQuery.GetServerInstance(Game.Arma_3, "164.132.202.63", 2303, false, 1000, 1000, 0, false);
                    ServerInfo info3 = gta3.GetInfo();
                    if (info3 != null)
                    {
                        if (info3.Players + 1 != info3.MaxPlayers)
                        {
                            win.Dispatcher.BeginInvoke((Action)(() => win.textBlockServer3.Text = $"{info3.Name} Players: {info3.Players + 1}/{info3.MaxPlayers}"));
                            win.Dispatcher.BeginInvoke((Action)(() => win.JoinServer3Button.IsEnabled = true));
                        }
                        else
                        {
                            win.Dispatcher.BeginInvoke((Action)(() => win.textBlockServer3.Text = "Server full"));
                            win.Dispatcher.BeginInvoke((Action)(() => win.JoinServer3Button.IsEnabled = false));
                        }
                    }
                    else
                    {
                        win.Dispatcher.BeginInvoke((Action)(() => win.textBlockServer3.Text = "Server offline"));
                        win.Dispatcher.BeginInvoke((Action)(() => win.JoinServer3Button.IsEnabled = false));
                    }
                    Task.Delay(2000);
                }
            });
        }
    }
}
