using System;
using Xamarin.Forms;

namespace ArtScanner.Models
{
    public class BookletItem : BaseModel
    {
        public string Title { get; set; }

        public Color BookletColor { get; set; }

        public byte[] ImageByteArray { get; set; }

    }
}
