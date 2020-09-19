using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArtScanner.Models
{
    public class GeneralItemInfoModel
    {
        [JsonProperty("defaultTitle")]
        public string DefaultTitle { get; set; }

        [JsonProperty("languages")]
        public List<string> Languages { get; set; }
    }
}
