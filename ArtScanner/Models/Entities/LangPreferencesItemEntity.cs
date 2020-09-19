using System;
using System.Globalization;
using SQLite;

namespace ArtScanner.Models.Entities
{
    public class LangPreferencesItemEntity : BaseEntity
    {
        [Ignore]
        public CultureInfo SelectedCulture { get; set; }

        public string NativeName { get; set; }

        public string LangTag { get; set; }

        public int SelectedCultureIndex { get; set; }

    }
}
