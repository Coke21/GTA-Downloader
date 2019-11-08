using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using GTADownloader.Classes;

namespace GTADownloader
{
    class Update
    {
        private static MainWindow Win = (MainWindow)Application.Current.MainWindow;

        public static async Task FilesCheckAsync(CancellationToken cancellationToken)
        {
            Win.LvName.IsEnabled = false;

            var listRequest = Data.Service.Files.List();
            listRequest.OrderBy = "name";
            listRequest.Fields = "files(id, name, modifiedTime)";
            listRequest.Q = $"'{Data.FolderId}' in parents";
            var files = await listRequest.ExecuteAsync();

            files.Files.Remove(files.Files.Single(r => r.Name == "readme.txt"));

            if (Win.Items.Count > 0)
            {
                await Task.Run(async () =>
                {
                    foreach (var item in Win.Items.ToList())
                    {
                        string status = await LvItemsCheckAsync(item.Mission, item.FileId);
                        item.IsMissionUpdated = status;
                        item.IsModifiedTimeUpdated = status;
                    }
                });

                //Mission file added to the Google drive
                if (Win.Items.Count < files.Files.Count)
                    foreach (var item in files.Files.ToList())
                    {
                        if (Win.Items.Any(a => a.Mission == item.Name)) continue;

                        int itemPosition = files.Files.IndexOf(item);
                        Win.Items.Insert(itemPosition, new ListViewProperties.ListViewItems
                        {
                            Mission = item.Name,
                            IsMissionUpdated = "Missing",
                            ModifiedTime = item.ModifiedTime.Value.ToString("dd.MM.yyyy HH:mm:ss"),
                            IsModifiedTimeUpdated = "Missing",
                            FileId = item.Id,
                            IsChecked = false
                        });
                    }

                //Mission file removed from the Google drive
                if (Win.Items.Count > files.Files.Count)
                    foreach (var item in Win.Items.ToList().Where(item => files.Files.All(a => a.Name != item.Mission)))
                    {
                        Win.Items.Remove(item);
                        if (File.Exists(DataProperties.GetArma3MissionFolderPath + item.Mission))
                            File.Delete(DataProperties.GetArma3MissionFolderPath + item.Mission);
                    }

                //The same amount in both places
                if (Win.Items.Count == files.Files.Count)
                    foreach (var (item1, item2) in Win.Items.Zip(files.Files, Tuple.Create).ToList().Where(item => item.Item1.ModifiedTime != item.Item2.ModifiedTime.Value.ToString("dd.MM.yyyy HH:mm:ss")))
                    {
                        string status = await LvItemsCheckAsync(item2.Name, item2.Id);
                        item1.Mission = item2.Name;
                        item1.IsMissionUpdated = status;
                        item1.ModifiedTime = item2.ModifiedTime.Value.ToString("dd.MM.yyyy HH:mm:ss");
                        item1.IsModifiedTimeUpdated = status;
                        item1.FileId = item2.Id;
                        item1.IsChecked = item1.IsChecked;
                    }
            }
            else
                foreach (var file in files.Files)
                {
                    string status = await LvItemsCheckAsync(file.Name, file.Id);
                    Win.Items.Add(new ListViewProperties.ListViewItems
                    {
                        Mission = file.Name,
                        IsMissionUpdated = status,
                        ModifiedTime = file.ModifiedTime.Value.ToString("dd.MM.yyyy HH:mm:ss"),
                        IsModifiedTimeUpdated = status,
                        FileId = file.Id,
                        IsChecked = false
                    });
                }

            Win.LvName.IsEnabled = true;

            var requestedProgram = await Data.GetFileRequest(Data.ProgramId, "md5Checksum").ExecuteAsync();
            string programMd5Checksum = CalculateMd5(DataProperties.GetProgramFolderPath + DataProperties.GetProgramName);

            if (cancellationToken.IsCancellationRequested) return;

            if (Equals(requestedProgram.Md5Checksum, programMd5Checksum))
                Win.TextTopOperationProgramNotice.Inlines.Add(new Run($"Updated") { Foreground = Brushes.ForestGreen});
            else
            {
                Win.TextTopOperationProgramNotice.Inlines.Add(new Run($"Outdated") { Foreground = Brushes.Red });

                Win.ProgramUpdateName.Visibility = Visibility.Visible;

                var result = System.Windows.Forms.MessageBox.Show("A new update for GTA program has been detected. Download it?", "Update", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    await Download.FileAsync(Data.ProgramId, null, Data.CtsStopDownloading.Token, "programUpdate");
            }
        }

        public static async Task<string> LvItemsCheckAsync(string fileName, string fileId)
        {
            var requestedFile = await Data.GetFileRequest(fileId, "md5Checksum").ExecuteAsync();

            string filePath = Path.Combine(DataProperties.GetArma3MissionFolderPath, fileName);
            string fileMd5Checksum = string.Empty;
            try
            {
                fileMd5Checksum = CalculateMd5(filePath);
            }
            catch (FileNotFoundException)
            {
                return "Missing";
            }

            return Equals(requestedFile.Md5Checksum, fileMd5Checksum) ? "Updated" : "Outdated";
        }
        private static string CalculateMd5(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public static async Task UpdateLvItemsCheckAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (ListViewProperties.SelectedCheckboxes == null || !ListViewProperties.SelectedCheckboxes.Any())
                {
                    await Task.Delay(5_000);
                    continue;
                }

                foreach (var selectedCheckbox in ListViewProperties.SelectedCheckboxes)
                {
                    string status = await LvItemsCheckAsync(selectedCheckbox.Mission, selectedCheckbox.FileId);

                    if (status.Equals("Updated")) continue;

                    await Download.FileAsync(selectedCheckbox.FileId, selectedCheckbox, Data.CtsAutomaticUpdate.Token);
                }
                //5 min
                await Task.Delay(300_000);
            }
        }
    }
}