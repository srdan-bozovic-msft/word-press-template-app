using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Disqus.Data
{
    public class Result<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("response")]
        public T Response { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return Code == 0;
            }
        }

        public string ErrorMessage { get; set; }
    }
}
