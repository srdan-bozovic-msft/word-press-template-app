using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Disqus.Data
{
    public class Avatar
    {
        [JsonProperty("small")]
        public Image Small { get; set; }

        [JsonProperty("isCustom")]
        public bool IsCustom { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("cache")]
        public string Cache { get; set; }

        [JsonProperty("large")]
        public Image Large { get; set; }
    }
}
