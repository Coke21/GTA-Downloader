using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GTADownloader.MyProperties
{
    public class AccentsProperties
    {
        public class AccentsItems
        {
            private string _colorName;
            public string ColorName
            {
                get => _colorName;
                set { _colorName = value; OnPropertyChanged(); }
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
