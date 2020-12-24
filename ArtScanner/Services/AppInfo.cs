using System;
namespace ArtScanner.Services
{
    //TODO: Bind this data to burger menu page
    class AppInfo : IAppInfo
    {
        public string Version => Xamarin.Essentials.AppInfo.VersionString;
        public string Build => Xamarin.Essentials.AppInfo.BuildString;
    }
}