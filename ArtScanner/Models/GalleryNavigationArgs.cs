using System;
using ArtScanner.Models.Entities;

namespace ArtScanner.Models
{
    public class GalleryNavigationArgs
    {
        public ItemEntity NavigatedModel { get; set; }

        public ItemEntity SelectedChildModel { get; set; }
    }
}
