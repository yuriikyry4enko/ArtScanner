using System;
using SQLite;

namespace ArtScanner.Models.Entities
{
    public class BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public long LocalId { get; set; }
    }
}
