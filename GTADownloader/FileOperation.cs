using System.IO;
using System.Linq;

namespace GTADownloader
{
    class FileOperation
    {
        public static void AppendFileLine(string line)
        {
            IsFilePresent("lv");
            using (var tw = new StreamWriter(Data.getListViewFilePath, true))
            {
                tw.WriteLine($"{line}");
            }
        }
        public static void DeleteFileLine(string deleteLine)
        {
            IsFilePresent("lv");
            File.WriteAllLines(Data.getListViewFilePath, File.ReadLines(Data.getListViewFilePath).Where(lines => lines != deleteLine).ToList());
        }
        public static void EditFileLine(string oldValue, string newValue)
        {
            string text = File.ReadAllText(Data.getConfigFilePath);
            text = text.Replace(oldValue, newValue);
            File.WriteAllText(Data.getConfigFilePath, text);
        }
        public static void IsFilePresent(string type)
        {
            if (!Directory.Exists(Data.getProgramFolderPath))
                Directory.CreateDirectory(Data.getProgramFolderPath);

            switch (type)
            {
                case "lv":
                    if (!File.Exists(Data.getListViewFilePath))
                    {
                        string[] lines = { "|ListView Data automatically created by GTA Mission Downloader.",
                                            "|Don't touch, unless you know what to do.",
                                            "|You can manually add/edit channel paths below.",
                                            "--------------------------------------------------------"};
                        File.AppendAllLines(Data.getListViewFilePath, lines);
                    }
                    break;
                case "config":
                    if (!File.Exists(Data.getConfigFilePath))
                    {
                        string[] lines = { "|Config file automatically created by GTA Mission Downloader.",
                                            "|Don't touch, unless you know what to do.",
                                            "|Default Lv channel & password - string, rest - 0 or 1",
                                            "---------------------------------------------------------",
                                            "Default Lv channel=",
                                            "Default Lv password=",
                                            "Run Hidden=0",
                                            "Run TS Auto=0",
                                            "S1Altis=0",
                                            "S2Altis=0",
                                            "S3Tanoa=0",
                                            "S3Malden=0",
                                            "Notification=0",
                                            "Automatic Update=0",
                                            "Download Speed=1"};
                        File.AppendAllLines(Data.getConfigFilePath, lines);
                    }
                    break;
            }
        }
    }
}
