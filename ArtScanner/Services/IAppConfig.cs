using System;
namespace ArtScanner.Services
{
    interface IAppConfig
    {

        string RootFolderName { get; }

        string RootPath { get; }

        string ImagesFolderName { get; }

        string AudioFolderName { get; }

        string RootFolderPath { get; }

        string DatabaseName { get; }

        bool NewDBEachAppVersion { get; }
    }
}
