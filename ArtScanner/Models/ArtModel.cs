using System;
using System.IO;

namespace ArtScanner.Models
{
    public class ArtModel : BaseModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string MusicUrl { get; set; }
        public string ImageUrl { get; set; }
        public string WikiUrl { get; set; }
        public bool Liked { get; set; }


        public byte[] ImageByteArray { get; set; }
        public byte[] MusicByteArray { get; set; }
        public string MusicFileName { get; set; }


    }
}
