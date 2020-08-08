using System;
namespace ArtScanner.Services
{
    class AppInfo : IAppInfo
    {
        public string Version => Xamarin.Essentials.AppInfo.VersionString;
        public string Build => Xamarin.Essentials.AppInfo.BuildString;
    }
}