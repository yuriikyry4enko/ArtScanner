using System;
using System.Threading.Tasks;
using ArtScanner.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ArtScanner.Droid.Services.DownloadFileService))]
namespace ArtScanner.Droid.Services
{
    public class DownloadFileService : IDownloadFileService
    {
        public void DownloadFile(string url)
        {
            BackgroundAudioFileAsyncTask task = new BackgroundAudioFileAsyncTask();
            task.Execute(new String[]
            {
                url
            });
        }
    }
}
