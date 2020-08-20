using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArtScanner.Services
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {

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
                        return await response.Content.ReadAsStringAsync(); ;
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