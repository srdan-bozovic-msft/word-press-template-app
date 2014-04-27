using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Disqus.Data
{
    public class Cursor
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("hasPrev")]
        public bool HasPrevious { get; set; }

        [JsonProperty("prev")]
        public string PreviousId { get; set; }

        [JsonProperty("hasNext")]
        public bool HasNext { get; set; }

        [JsonProperty("next")]
        public string NextId { get; set; }

        [JsonProperty("more")]
        public bool HasMore { get; set; }

        //total?
    }
}
