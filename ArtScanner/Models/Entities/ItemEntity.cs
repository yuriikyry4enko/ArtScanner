using System;
using SQLite;

namespace ArtScanner.Models.Entities
{
    public class ItemEntity : BaseEntity
    {
        // Server Id
        public string Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string MusicUrl { get; set; }
        public string ImageUrl { get; set; }
        public string WikiUrl { get; set; }

    }
}
