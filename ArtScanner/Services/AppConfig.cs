using System;
using System.IO;
using Xamarin.Essentials;

namespace ArtScanner.Services
{
    class AppConfig : IAppConfig
    {
        public string RootFolderName => "artscanner";
        public string RootPath => FileSystem.AppDataDirectory;
        public string RootFolderPath => Path.Combine(RootPath, RootFolderName);
        public string ImagesFolderName => "img_files";
        public string AudioFolderName => "mp3_files";
        public string DatabaseName => "database";
        public bool NewDBEachAppVersion => true;
    }
}
