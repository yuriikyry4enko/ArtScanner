using System;
using System.IO;

namespace ArtScanner.Services
{
    public enum FileType
    {
        Image,
        Audio,
    }

    class AppFileSystemService : IAppFileSystemService
    {
        private readonly IAppConfig appConfig;

        public string CurrentUserFolderPath { get; private set; }

        public void InitializeFoldersForUser(string userName)
        {
            var userFolderPath = Path.Combine(appConfig.RootFolderPath, userName);

            Directory.CreateDirectory(userFolderPath);

            CurrentUserFolderPath = userFolderPath;

            var imagesFolderPath = GetImagesFolderPath();
            var audioFolderPath = GetAudioFolderPath();

            Directory.CreateDirectory(imagesFolderPath);
            Directory.CreateDirectory(audioFolderPath);
        }

        public string SaveImage(byte[] data, string name)
        {
            var imagesFolderPath = GetImagesFolderPath();

            var newImagePath = Path.Combine(imagesFolderPath, name);

            File.WriteAllBytes(newImagePath, data);

            return newImagePath;
        }

        public string SaveAudio(byte[] data, string name)
        {
            var audioFolderPath = GetAudioFolderPath();

            var newAudioPath = Path.Combine(audioFolderPath, name);

            File.WriteAllBytes(newAudioPath, data);

            return newAudioPath;
        }

        public bool DoesAudioExist(string name)
        {
            if (name == null)
                return false;

            var audioFolderPath = GetAudioFolderPath();

            var audioPath = Path.Combine(audioFolderPath, name);

            return File.Exists(audioPath);
        }

        public bool DoesImageExist(string name)
        {
            if (name == null)
                return false;

            var imagesFolderPath = GetImagesFolderPath();

            var imagePath = Path.Combine(imagesFolderPath, name);

            return File.Exists(imagePath);
        }

        public string GetFilePath(string name, FileType fileType)
        {
            switch(fileType)
            {
                case FileType.Image:
                    var imagesFolderPath = GetImagesFolderPath();
                    return Path.Combine(imagesFolderPath, name);

                case FileType.Audio:
                    var audioFolderPath = GetAudioFolderPath();
                    return Path.Combine(audioFolderPath, name);

                default:
                    return string.Empty;
            }
            
        }

        public void DeleteFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;

            File.Delete(path);
        }

        private string GetImagesFolderPath()
        {
            return Path.Combine(CurrentUserFolderPath, appConfig.ImagesFolderName);
        }

        private string GetAudioFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        public AppFileSystemService
            (IAppConfig appConfig)
        {
            this.appConfig = appConfig;
        }
    }
}
