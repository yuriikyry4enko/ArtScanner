using System;
using System.IO;
using System.Threading.Tasks;

namespace ArtScanner.Services
{
    public interface IDownloadFileService
    {
        Task DownloadFile(string url, string fileName, Stream responseStream = null);
    }
}
