using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArtScanner.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        public long LocalId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}