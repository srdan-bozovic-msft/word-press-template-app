using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Disqus.Data
{
    public class PageResult<T> : Result<T[]>
    {
        [JsonProperty("cursor")]
        public Cursor Cursor { get; set; }
    }
}
