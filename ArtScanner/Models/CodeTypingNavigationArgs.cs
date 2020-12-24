using System;
using ArtScanner.Models.Entities;

namespace ArtScanner.Models
{
    public class CodeTypingNavigationArgs
    {
        public Action<ItemEntity> PageUpdated { get; set; }

        public ItemEntity ActiveFolderEntity { get; set; }
    }
}
