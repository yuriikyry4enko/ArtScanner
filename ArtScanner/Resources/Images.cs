﻿using System;
using System.Reflection;

namespace ArtScanner.Resources
{
    public static class Images
    {
        const string SvgResourcesUrl = "resource://ArtScanner.Resources";


        // Svg assembly resources
        public static string Play => GetSvgImageUrl("play");
        public static string Share => GetSvgImageUrl("share");
        public static string Like => GetSvgImageUrl("liked");
        public static string Twitter => GetSvgImageUrl("twitter");
        public static string Pause => GetSvgImageUrl("pause");
        public static string BackArrow => GetSvgImageUrl("back_arrow");
        public static string Scan => GetSvgImageUrl("scan");
        public static string DefaultLike => GetSvgImageUrl("like");
        public static string Settings => GetSvgImageUrl("settings");
        public static string Museum => GetSvgImageUrl("museum");
        public static string SadIdiom => GetSvgImageUrl("ideogram");
        public static string Flyer => GetSvgImageUrl("flyer");
        public static string Trash => GetSvgImageUrl("trash");
        public static string Menu => GetSvgImageUrl("menu");
        public static string Tick => GetSvgImageUrl("tick");
        public static string Cancel => GetSvgImageUrl("cancel");


        static string GetSvgImageUrl(string name) => $"{SvgResourcesUrl}.{name}.svg";
    }
}