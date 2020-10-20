using System;
namespace ArtScanner.Models
{
    public class MenuItem
    {
        public string Title { get; set; }

        public Action NavigationCommand { get; set; }
    }
}
