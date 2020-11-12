using System;
using System.IO;
using System.Threading.Tasks;
using ArtScanner.Models;

namespace ArtScanner.Services
{
    interface IRestService
    {
        Task<byte[]> GetImageById(long id);
        Task<TextItemInfoModel> GetTextById(string langTag, long id);
        Task<GeneralItemInfoModel> GetGeneralItemInfo(long id);
        Task<QRcodeDataResultModel> GetIdByQRCode(string qrcodeData);

    }
}
