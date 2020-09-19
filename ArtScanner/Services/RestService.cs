using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using ArtScanner.Models;
using Newtonsoft.Json;

namespace ArtScanner.Services
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {

        }

        public async Task<GeneralItemInfoModel> GetGeneralItemInfo(string id)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetGeneralItemInfo, id));
            try
            {
                using (client = new HttpClient())
                {
                    var result = await client.GetStringAsync(uri);
                    return JsonConvert.DeserializeObject<GeneralItemInfoModel>(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }

        public async Task<byte[]> GetImageById(string langTag, string id)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetJPGById, langTag, id));
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

        public async Task<TextItemInfoModel> GetTextById(string langTag, string id)
        {
            Uri uri = new Uri(string.Format(Utils.Constants.ApiConstants.GetTextById, langTag, id));
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return null;
        }
    }
}