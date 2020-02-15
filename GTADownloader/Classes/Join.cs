using System.Threading.Tasks;
using QueryMaster;
using QueryMaster.GameServer;
using System.Windows;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using AngleSharp;

namespace GTADownloader
{
    class Join
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;

        public static void Server(string server)
        {
            switch (server)
            {
                case "S1":
                    Process.Start(DataProperties.GetRegistryArma3ExePath, "-connect=164.132.200.53:2302");
                    break;
                case "S2":
                    Process.Start(DataProperties.GetRegistryArma3ExePath, "-connect=164.132.202.63:2602");
                    break;
                case "S3":
                    Process.Start(DataProperties.GetRegistryArma3ExePath, "-connect=164.132.202.63:2302");
                    break;
                case "TS":
                    Process.Start("ts3server://" + "TS.grandtheftarma.com:9987" + $"?channel={DataProperties.W2.InsertTsChannelName.Text}" + $"&channelpassword={DataProperties.W2.InsertTsChannelPasswordName.Text}");
                    break;
            }
        }

        public static async void UpdateServerAsync()
        {
            await Task.Run(() =>
            {
                Server gta1 = ServerQuery.GetServerInstance(Game.Arma_3, "164.132.200.53", 2303, false, 1000, 1000, 0);
                Server gta2 = ServerQuery.GetServerInstance(Game.Arma_3, "164.132.202.63", 2603, false, 1000, 1000, 0);
                Server gta3 = ServerQuery.GetServerInstance(Game.Arma_3, "164.132.202.63", 2303, false, 1000, 1000, 0);

                while (true)
                {
                    ServerInfo info1 = gta1.GetInfo();
                    ServerInfo info2 = gta2.GetInfo();
                    ServerInfo info3 = gta3.GetInfo();

                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(async () =>
                        {
                            ShowServerInfo(info1, Win.TextBlockServer1, Win.JoinServer1Button);

                            ShowServerInfo(info2, Win.TextBlockServer2, Win.JoinServer2Button);

                            ShowServerInfo(info3, Win.TextBlockServer3, Win.JoinServer3Button);

                            await ScrapGTA(Win.TextBlockTs);
                        });

                    Task.Delay(15_000);
                }
            });
        }
        private static void ShowServerInfo(ServerInfo info, TextBlock textBlock, Button button)
        {
            if (info != null)
            {
                if (info.Players + 1 != info.MaxPlayers)
                {
                     textBlock.Text = $"{info.Name} | Players: {info.Players + 1}/{info.MaxPlayers}";
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
        private static async Task ScrapGTA(TextBlock textBlock)
        {
            await Task.Run(async () =>
            {
                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);
                var document = await context.OpenAsync("https://grandtheftarma.com/");
                //How to get cellSelector: go to website, f12, elements, right click on smt, copy, copy selector
                var cellSelector = "#container > div:nth-child(4) > a > span.node.right > b";
                var cells = document.QuerySelectorAll(cellSelector);
                var stuff = cells.Select(m => m.TextContent);

                foreach (var number in stuff)
                    Win.Dispatcher.Invoke(() => textBlock.Text = $"TeamSpeak: {number}");
            });
        }
    }
}
