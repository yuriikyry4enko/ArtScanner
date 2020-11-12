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
        public IEnumerable<string> Languages { get; set; }

        [JsonProperty("parentId")]
        public long? ParentId { get; set; }

        [JsonProperty("isFolder")]
        public bool IsFolder { get; set; }
    }
}
