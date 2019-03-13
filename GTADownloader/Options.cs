using Microsoft.Win32;
using System;
using System.Windows;

namespace GTADownloader
{
    class Options
    {
        private static MainWindow win = (MainWindow)System.Windows.Application.Current.MainWindow;

        public static void Choose (string whichOption)
        {
            RegistryKey keyStartUp = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            RegistryKey keyDeleteValue = Registry.CurrentUser.OpenSubKey(@"Software\GTAProgram", true);
            RegistryKey keyRemoveSubKey = Registry.CurrentUser.OpenSubKey(@"Software\", true);

            switch (whichOption)
            {
                case "startUp":
                    keyStartUp.SetValue("GTADownloader", System.Reflection.Assembly.GetExecutingAssembly().Location);
                    break;
                case "startUpUnCheck":
                    keyStartUp.DeleteValue("GTADownloader");
                    break;
                case "runHidden":
                    win.StartUpCheckBox.IsChecked = true;
                    win.StartUpCheckBox.IsEnabled = false;
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Run Hidden", "Yes");
                    break;
                case "runHiddenUnCheck":
                    win.StartUpCheckBox.IsEnabled = true;
                    keyDeleteValue.DeleteValue("Run Hidden");
                    break;
                case "S1Altis":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S1Altis", "On");
                    break;
                case "S1AltisUnCheck":
                    keyDeleteValue.DeleteValue("S1Altis");
                    break;
                case "S2Altis":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S2Altis", "On");
                    break;
                case "S2AltisUnCheck":
                    keyDeleteValue.DeleteValue("S2Altis");
                    break;
                case "S3Tanoa":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Tanoa", "On");
                    break;
                case "S3TanoaUnCheck":
                    keyDeleteValue.DeleteValue("S3Tanoa");
                    break;
                case "S3Malden":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Malden", "On");
                    break;
                case "S3MaldenUnCheck":
                    keyDeleteValue.DeleteValue("S3Malden");
                    break;
                case "notification":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Notification", "On");
                    break;
                case "notificationUnCheck":
                    keyDeleteValue.DeleteValue("Notification");
                    break;
                case "automaticUpdate":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Automatic Update", "On");
                    break;
                case "automaticUpdateUnCheck":
                    keyDeleteValue.DeleteValue("Automatic Update");
                    break;
                case "maxSpeed":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Download Speed", "Max");
                    break;
                case "normalSpeed":
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Download Speed", "Normal");
                    break;
                case "removeRegistry":
                    try
                    {
                        keyRemoveSubKey.DeleteSubKey("GTAProgram");
                        System.Windows.MessageBox.Show("All changes made to your registry have been deleted.", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    catch (ArgumentException exc)
                    {
                        System.Windows.MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
            }
        }
        public static void UpdateCheckBoxes()
        {
            object startupValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "GTADownloader", null);
            if (startupValue == null) win.StartUpCheckBox.IsChecked = false;
            else win.StartUpCheckBox.IsChecked = true;

            object runMinimizedValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Run Hidden", null);
            if (runMinimizedValue == null)
            {
                win.HiddenCheckBox.IsChecked = false;
                win.WindowState = WindowState.Normal;
            }
            else
            {
                win.HiddenCheckBox.IsChecked = true;
                win.ShowInTaskbar = false;
                win.Hide();
            }

            object notificationValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Notification", null);
            if (notificationValue == null) win.NotificationCheckBox.IsChecked = false;
            else win.NotificationCheckBox.IsChecked = true;

            string downloadSpeed = "";
            try
            {
                downloadSpeed = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Download Speed", "Max").ToString();
            }
            catch (NullReferenceException)
            {
                win.MaxSpeedButton.IsChecked = true;
            }
            finally
            {
                if (downloadSpeed == "Max") win.MaxSpeedButton.IsChecked = true;
                else if (downloadSpeed == "Normal") win.NormalSpeedButton.IsChecked = true;
            }

            object automaticUpdateValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "Automatic Update", null);
            if (automaticUpdateValue == null) win.AutomaticUpdateCheckBox.IsChecked = false;
            else win.AutomaticUpdateCheckBox.IsChecked = true;

            object S1AltisValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S1Altis", null);
            if (S1AltisValue == null) win.S1AltisCheckBox.IsChecked = false;
            else win.S1AltisCheckBox.IsChecked = true;

            object S2AltisValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S2Altis", null);
            if (S2AltisValue == null) win.S2AltisCheckBox.IsChecked = false;
            else win.S2AltisCheckBox.IsChecked = true;

            object S3TanoaValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Tanoa", null);
            if (S3TanoaValue == null) win.S3TanoaCheckBox.IsChecked = false;
            else win.S3TanoaCheckBox.IsChecked = true;

            object S3MaldenValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\GTAProgram", "S3Malden", null);
            if (S3MaldenValue == null) win.S3MaldenCheckBox.IsChecked = false;
            else win.S3MaldenCheckBox.IsChecked = true;
        }   
    }
}
