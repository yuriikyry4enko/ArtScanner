﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using ArtScanner.Models;
using ArtScanner.Models.Analytics;
using Newtonsoft.Json;

namespace ArtScanner.Services
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {

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
    }
}