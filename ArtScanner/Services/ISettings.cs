using System;
namespace ArtScanner.Services
{
    public interface ISettings
    {
        bool IsUserFolderInitialized { get; set; }
    }
}
