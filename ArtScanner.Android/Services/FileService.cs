using System;
using System.IO;
using ArtScanner.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ArtScanner.Droid.Services.FileService))]
namespace ArtScanner.Droid.Services
{
    public class FileService : IFileService
    {
        public string GetAbsolutPath(string filename)
        {
            string directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            string filePath = Path.Combine(directory, filename);

            return filePath;
        }
    }
}
