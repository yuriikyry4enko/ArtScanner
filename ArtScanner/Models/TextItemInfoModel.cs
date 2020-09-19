using System;
using Newtonsoft.Json;

namespace ArtScanner.Models
{
    public class TextItemInfoModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }
    }
}
