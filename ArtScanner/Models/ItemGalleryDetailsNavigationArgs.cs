using System;
using System.Collections.Generic;
using ArtScanner.Models.Entities;

namespace ArtScanner.Models
{
    public class ItemGalleryDetailsNavigationArgs
    {
        public ItemEntity ItemModel { get; set; }

        public Action NeedsToUpdatePrevious { get; set; }

        public IEnumerable<string> ItemLanguages { get; set; }

    }
}
