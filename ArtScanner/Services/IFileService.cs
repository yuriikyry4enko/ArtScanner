using System;
namespace ArtScanner.Services
{
    public interface IFileService
    {
        string GetAbsolutPath(string filename);
    }
}
