﻿using System;
namespace ArtScanner.Models
{
    public class LoadingNavigationArgs
    {
        public Action PageLoadingCanceled { get; set; }

        public bool IsCanceled { get; set; }
    }
}
