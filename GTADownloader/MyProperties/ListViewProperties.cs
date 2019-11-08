using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GTADownloader.Classes
{
    public class ListViewProperties
    {
        public static IEnumerable<ListViewItems> SelectedItems { get; set; }
        public static IEnumerable<ListViewItems> SelectedCheckboxes { get; set; }

        public class ListViewItems : INotifyPropertyChanged
        {
            private string _mission;
            private string _isMissionUpdated;
            private string _fileId;
            private string _modifiedTime;
            private string _isModifiedTimeUpdated;
            private bool _isChecked;

            public string Mission
            {
                get => _mission;
                set { _mission = value; OnPropertyChanged(); }
            }

            public string IsMissionUpdated
            {
                get => _isMissionUpdated;
                set { _isMissionUpdated = value; OnPropertyChanged(); }
            }

            public string FileId
            {
                get => _fileId;
                set { _fileId = value; OnPropertyChanged(); }
            }

            public string ModifiedTime
            {
                get => _modifiedTime;
                set { _modifiedTime = value; OnPropertyChanged(); }
            }

            public string IsModifiedTimeUpdated
            {
                get => _isModifiedTimeUpdated;
                set { _isModifiedTimeUpdated = value; OnPropertyChanged(); }
            }

            public bool IsChecked
            {
                get => _isChecked;
                set { _isChecked = value; OnPropertyChanged(); }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChangedEventHandler handler = PropertyChanged;

                handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            } 
        }
    }
}
