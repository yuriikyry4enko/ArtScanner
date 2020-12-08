using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ArtScanner.Models
{
    public class CultureInfoModelExtended : CultureInfo, INotifyPropertyChanged
    {

        private bool _isChecked = false;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public CultureInfoModelExtended(int culture) : base(culture)
        {
        }

        public CultureInfoModelExtended(string name) : base(name)
        {
        }

        public CultureInfoModelExtended(int culture, bool useUserOverride) : base(culture, useUserOverride)
        {
        }

        public CultureInfoModelExtended(string name, bool useUserOverride) : base(name, useUserOverride)
        {
        }
    }
}
