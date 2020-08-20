using System;
using System.IO;
using Xamarin.Essentials;

namespace ArtScanner.Services
{
    class AppConfig : IAppConfig
    {
        public string ServerUrl => "";
        public string RootFolderName => "artscanner";
        public string RootPath => FileSystem.AppDataDirectory;
        public string RootFolderPath => Path.Combine(RootPath, RootFolderName);
        public string ImagesFolderName => "files";
        public string DatabaseName => "database";
        public bool NewDBEachAppVersion => true;
    }
}
