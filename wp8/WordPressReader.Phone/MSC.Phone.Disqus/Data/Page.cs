using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Disqus.Data
{
    public class Page<T>
    {
        [JsonProperty("cursor")]
        public Cursor Cursor { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("response")]
        public T[] Response { get; set; }
    }
}
