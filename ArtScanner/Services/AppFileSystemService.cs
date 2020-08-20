using System;
using System.IO;

namespace ArtScanner.Services
{
    class AppFileSystemService : IAppFileSystemService
    {
        private readonly IAppConfig appConfig;

        public string CurrentUserFolderPath { get; private set; }


        //public void EnsureFileSystemTreeStructure()
        //{
        //    Directory.CreateDirectory(appConfig.RootFolderPath);
        //}

        public void InitializeFoldersForUser(string userName)
        {
            var userFolderPath = Path.Combine(appConfig.RootFolderPath, userName);

            Directory.CreateDirectory(userFolderPath);

            CurrentUserFolderPath = userFolderPath;

            var imagesFolderPath = GetImagesFolderPath();

            Directory.CreateDirectory(imagesFolderPath);
        }

        //public string CopyImageIntoUserFolderIfNeeded(string imagePath)
        //{
        //    var folder = Path.GetDirectoryName(imagePath);

        //    var imagesFolderPath = GetImagesFolderPath();

        //    if (folder == imagesFolderPath) return imagePath;

        //    var newImagePath = Path.Combine(imagesFolderPath, $"image_{DateTime.UtcNow:yyyy_MM_dd_HH_mm_ss_fff}");

        //    File.Copy(imagePath, newImagePath, true);

        //    return newImagePath;
        //}

        public string SaveImage(byte[] data, string name)
        {
            var imagesFolderPath = GetImagesFolderPath();

            var newImagePath = Path.Combine(imagesFolderPath, name);

            File.WriteAllBytes(newImagePath, data);

            return newImagePath;
        }

        public string SaveStreamAudio(byte[] data, string name)
        {
            var imagesFolderPath = GetImagesFolderPath();

            var newImagePath = Path.Combine(imagesFolderPath, name);

            File.WriteAllBytes(newImagePath, data);

            return newImagePath;
        }

        //public string Rename(string path, string name)
        //{
        //    var imagesFolderPath = GetImagesFolderPath();

        //    var newImagePath = Path.Combine(imagesFolderPath, name);

        //    File.Move(path, newImagePath);

        //    return newImagePath;
        //}

        public bool DoesImageExist(string name)
        {
            var imagesFolderPath = GetImagesFolderPath();

            var imagePath = Path.Combine(imagesFolderPath, name);

            return File.Exists(imagePath);
        }

        public string GetFilePath(string name)
        {
            var imagesFolderPath = GetImagesFolderPath();

            return Path.Combine(imagesFolderPath, name);
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


        public AppFileSystemService
            (IAppConfig appConfig)
        {
            this.appConfig = appConfig;
        }
    }
}
