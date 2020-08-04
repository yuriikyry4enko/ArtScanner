using System;
using System.Reflection;

namespace ArtScanner.Resources
{
    public static class Images
    {
        const string SvgResourcesUrl = "resource://ArtScanner.Resources";


        // Svg assembly resources
        public static string Play => GetSvgImageUrl("play");
        public static string Share => GetSvgImageUrl("share");
        public static string Like => GetSvgImageUrl("like");
        public static string Twitter => GetSvgImageUrl("twitter");
        public static string Pause => GetSvgImageUrl("pause");
        public static string BackArrow => GetSvgImageUrl("back_arrow");
        public static string Scan => GetSvgImageUrl("scan");
        public static string DefaultLike => GetSvgImageUrl("default_like");

        static string GetSvgImageUrl(string name) => $"{SvgResourcesUrl}.{name}.svg";
    }
}