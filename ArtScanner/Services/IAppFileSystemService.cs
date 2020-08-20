using System;
namespace ArtScanner.Services
{
    interface IAppFileSystemService
    {
        string CurrentUserFolderPath { get; }

        void EnsureFileSystemTreeStructure();

        void InitializeFoldersForUser(string userName);

        // Returns imagePath if everything is OK or new path if copied
        string CopyImageIntoUserFolderIfNeeded(string imagePath);

        string SaveImage(byte[] date, string name);

        string Rename(string path, string name);

        bool DoesImageExist(string name);

        string GetFilePath(string name);

        void DeleteFile(string path);

        string SaveStreamAudio(byte[] data, string name);
    }
}
