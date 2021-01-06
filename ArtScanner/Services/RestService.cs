using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ArtScanner.Models;
using ArtScanner.Models.Analytics;
using ArtScanner.Utils.Constants;
using Newtonsoft.Json;

namespace ArtScanner.Services
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {

        }

        public async Task<QRcodeDataResultModel> GetItemIdByShortCode(string itemShortCode, long folderId)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetItemIdBYShortCode, folderId, itemShortCode));
            try
            {
                using (var bench = new Benchmark($"Get itemid by shortcode = {itemShortCode} and folder {folderId}"))
                {
                    using (client = new HttpClient())
                    {
                        var result = await client.GetStringAsync(uri);
                        return JsonConvert.DeserializeObject<QRcodeDataResultModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }

            return null;
        }

        public async Task<GeneralItemInfoModel> GetGeneralItemInfo(long itemId)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetGeneralById, itemId));
            try
            {
                using (var bench = new Benchmark($"Get general info by id = {itemId}"))
                {
                    using (client = new HttpClient())
                    {
                        var result = await client.GetStringAsync(uri);
                        return JsonConvert.DeserializeObject<GeneralItemInfoModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }

            return null;
        }

        public async Task<byte[]> GetImageById(long id)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetJPGById, id));
            try
            {
                using (var bench = new Benchmark($"Get Image by id = {id}"))
                {
                    using (client = new HttpClient())
                    {
                        return await client.GetByteArrayAsync(uri); ;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }

            return null;
        }

        public async Task<TextItemInfoModel> GetTextById(string langTag, long id)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetTextById, langTag, id));
            try
            {
                using (var bench = new Benchmark($"Get text by id = {id} and tag = {langTag}"))
                {
                    using (client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(uri);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = await client.GetStringAsync(uri);
                            return JsonConvert.DeserializeObject<TextItemInfoModel>(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }

            return null;
        }

        public async Task<QRcodeDataResultModel> GetIdByQRCode(string qrcodeData)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetIdByQRCode, qrcodeData));
            try
            {
                using (client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await client.GetStringAsync(uri);
                        return JsonConvert.DeserializeObject<QRcodeDataResultModel>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }

            return null;
        }

        public async Task<Stream> GetMusicStreamById(long id, string langTag)
        {
            try
            {
                Stream stream = null;

                using (var bench = new Benchmark($"Get and save (audio) stream by id = {id} and langTag = {langTag}"))
                {
                    using ( client = new HttpClient())
                    {
                        stream = await client.GetStreamAsync(string.Format(ApiConstants.GetAudioStreamById, langTag, id));
                    }
                }

                return stream;
            }
            catch(Exception ex)
            {
                LogService.Log(ex);
            }

            return null;
        }
    }


    public class QueueStream : Stream
    {
        Stream writeStream;
        Stream readStream;
        long size;
        bool done;
        object plock = new object();

        public QueueStream(string storage)
        {
            writeStream = new FileStream(storage, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 4096);
            readStream = new FileStream(storage, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return readStream.Length; }
        }

        public override long Position
        {
            get { return readStream.Position; }
            set { throw new NotImplementedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (plock)
            {
                while (true)
                {
                    if (Position < size)
                    {
                        int n = readStream.Read(buffer, offset, count);
                        return n;
                    }
                    else if (done)
                        return 0;

                    try
                    {
                        Debug.WriteLine("Waiting for data");
                        Monitor.Wait(plock);
                        Debug.WriteLine("Waking up, data available");
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void Push(byte[] buffer, int offset, int count)
        {
            lock (plock)
            {
                writeStream.Write(buffer, offset, count);
                size += count;
                writeStream.Flush();
                Monitor.Pulse(plock);
            }
        }

        public void Done()
        {
            lock (plock)
            {
                Monitor.Pulse(plock);
                done = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                readStream.Close();
                readStream.Dispose();
                writeStream.Close();
                writeStream.Dispose();
            }
            base.Dispose(disposing);
        }

        #region non implemented abstract members of Stream

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}