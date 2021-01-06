using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Android.OS;
using ArtScanner.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ArtScanner.Droid.Services.DownloadFileService))]
namespace ArtScanner.Droid.Services
{
    public class DownloadFileService : IDownloadFileService
    {
        BackgroundAudioFileAsyncTask task;

        public async Task DownloadFile(string url, string fileName, Stream responseStream = null)
        {
            try
            {
                if (task == null)
                {
                    task = new BackgroundAudioFileAsyncTask();
                }
                else
                {
                    task.Cancel(false);
                    task = new BackgroundAudioFileAsyncTask();
                }

                task.Execute(new String[]
                {
                    url,
                    fileName
                });
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
        }
    }
}
