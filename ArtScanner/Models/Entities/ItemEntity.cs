using System;
using SQLite;

namespace ArtScanner.Models.Entities
{
    public class ItemEntity : BaseEntity
    {
        // Server Id
        public string Id { get; set; }

        public long ParentId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string MusicUrl { get; set; }
        public string ImageUrl { get; set; }
        public string WikiUrl { get; set; }


        public string LangTag { get; set; }

        public bool Liked { get; set; }


        [Ignore]
        public byte[] ImageByteArray { get; set; }

        [Ignore]
        public byte[] MusicByteArray { get; set; }

        public string MusicFileName { get; set; }

        public string ImageFileName { get; set; }


    }
}
