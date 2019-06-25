using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;

namespace GTADownloader
{
    class DataProperties
    {
        public static TSWindow W2 { get; set; } = new TSWindow();

        public static string GetArma3FolderPath { get; } = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/Arma 3";
        public static string GetArma3MissionFolderPath { get; } = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/Arma 3/MPMissionsCache/";

        public static string GetProgramDataFolderPath { get; } = @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/GTADownloader";

        public static string GetRegistryArma3EXEPath { get; } = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\bohemia interactive\arma 3", "main", string.Empty).ToString() + @"\arma3battleye";

        public static string GetProgramFolderPath { get; } = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\";
        public static string GetProgramName { get; } = AppDomain.CurrentDomain.FriendlyName;

        public static string DownloadSpeed { get; set; }
    }
}
