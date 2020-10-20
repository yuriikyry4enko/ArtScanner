using System;
using Newtonsoft.Json;

namespace ArtScanner.Models
{
    public class QRcodeDataResultModel
    {
        [JsonProperty("itemId")]
        public long ItemId { get; set; }
    }
}
