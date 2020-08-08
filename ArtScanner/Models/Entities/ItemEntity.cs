using System;
using SQLite;

namespace ArtScanner.Models.Entities
{
    public class ItemEntity
    {
        [PrimaryKey, AutoIncrement]
        public long LocalId { get; set; }

        // Server Id
        public long Id { get; set; }
    }
}
