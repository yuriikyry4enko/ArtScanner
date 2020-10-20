using System;
using SQLite;

namespace ArtScanner.Models.Entities
{
    public class CategoryItemEntity : BaseEntity
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public long ParentId { get; set; }

        [Ignore]
        public byte[] ImageByteArray { get; set; }

        public string ImageFileName { get; set; }

    }
}
