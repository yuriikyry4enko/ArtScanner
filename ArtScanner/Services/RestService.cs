using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ArtScanner.Services
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {

        }

        public async Task<byte[]> GetMusicStreamById(string id)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetAudioStreamById, id));
            try
            {
                using (client = new HttpClient())
                {
                    return await client.GetByteArrayAsync(uri); ;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        public async Task<byte[]> GetImageById(string id)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetJPGById, id));
            try
            {
                using (client = new HttpClient())
                {
                    return await client.GetByteArrayAsync(uri); ;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        public async Task<string> GetTextById(string id)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetTextById, id));
            try
            {
                using (client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        return content;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }
    }
}