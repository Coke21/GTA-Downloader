using System.IO;
using System.Linq;

namespace GTADownloader
{
    class FileOperation
    {
        public static void AppendFileLine(string line)
        {
            IsFilePresent("lv");
            using (var tw = new StreamWriter(Data.listViewFilePath, true))
            {
                tw.WriteLine($"{line}");
            }
        }
        public static void DeleteFileLine(string deleteLine)
        {
            IsFilePresent("lv");
            File.WriteAllLines(Data.listViewFilePath, File.ReadLines(Data.listViewFilePath).Where(lines => lines != deleteLine).ToList());
        }
        public static void EditFileLine(string oldValue, string newValue)
        {
            string text = File.ReadAllText(Data.configFilePath);
            text = text.Replace(oldValue, newValue);
            File.WriteAllText(Data.configFilePath, text);
        }
        public static void IsFilePresent(string type)
        {
            if (!Directory.Exists(Data.programFolderPath))
                Directory.CreateDirectory(Data.programFolderPath);

            switch (type)
            {
                case "lv":
                    if (!File.Exists(Data.listViewFilePath))
                    {
                        string[] lines = { "|ListView Data automatically created by GTA Mission Downloader.",
                                            "|Don't touch, unless you know what to do.",
                                            "|You can manually add/edit channel paths below.",
                                            "--------------------------------------------------------"};
                        File.AppendAllLines(Data.listViewFilePath, lines);
                    }
                    break;
                case "config":
                    if (!File.Exists(Data.configFilePath))
                    {
                        string[] lines = { "|Config file automatically created by GTA Mission Downloader.",
                                            "|Don't touch, unless you know what to do.",
                                            "|Default Lv channel - string, rest - 0 or 1",
                                            "---------------------------------------------------------",
                                            "Default Lv channel=",
                                            "Run Hidden=0",
                                            "Run TS Auto=0",
                                            "S1Altis=0",
                                            "S2Altis=0",
                                            "S3Tanoa=0",
                                            "S3Malden=0",
                                            "Notification=0",
                                            "Automatic Update=0",
                                            "Download Speed=1"};
                        File.AppendAllLines(Data.configFilePath, lines);
                    }
                    break;
            }
        }
    }
}
