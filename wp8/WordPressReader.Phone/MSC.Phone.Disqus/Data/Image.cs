using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Disqus.Data
{
    public class Image
    {
        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("cache")]
        public string Cache { get; set; }
    }
}
