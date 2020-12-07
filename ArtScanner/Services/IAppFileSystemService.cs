using System;
namespace ArtScanner.Services
{
    interface IAppFileSystemService
    {
        string CurrentUserFolderPath { get; }

        void InitializeFoldersForUser(string userName);

        string SaveImage(byte[] date, string name);

        string SaveAudio(byte[] data, string name);

        bool DoesImageExist(string name);

        bool DoesAudioExist(string name);

        string GetFilePath(string name, FileType fileType);

        void DeleteFile(string path);
    }
}
