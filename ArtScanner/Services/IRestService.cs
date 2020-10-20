using System;
using System.IO;
using System.Threading.Tasks;
using ArtScanner.Models;

namespace ArtScanner.Services
{
    interface IRestService
    {
        Task<byte[]> GetImageById(string langTag, string id);
        Task<TextItemInfoModel> GetTextById(string langTag, string id);
        Task<GeneralItemInfoModel> GetGeneralItemInfo(long id);
        Task<QRcodeDataResultModel> GetIdByQRCode(string qrcodeData);

    }
}
