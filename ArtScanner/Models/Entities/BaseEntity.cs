using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace ArtScanner.Models.Entities
{
    public class BaseEntity : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public long LocalId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
