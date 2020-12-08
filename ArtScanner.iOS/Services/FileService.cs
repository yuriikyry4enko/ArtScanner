using System;
using System.IO;
using ArtScanner.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ArtScanner.iOS.Services.FileService))]
namespace ArtScanner.iOS.Services
{
    public class FileService : IFileService
    {
        public string GetAbsolutPath(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename);
        }
    }
}
