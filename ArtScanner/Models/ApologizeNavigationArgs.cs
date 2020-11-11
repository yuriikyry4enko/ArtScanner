using System;
using System.Collections.Generic;
using System.Globalization;

namespace ArtScanner.Models
{
    public class ApologizeNavigationArgs
    {
        public IEnumerable<string> LanguageTags { get; set; }

        public Action<string> PopupResultAction { get; set; }

        public Action PageApologizeFinishedLoading { get; set; }
    }
}
