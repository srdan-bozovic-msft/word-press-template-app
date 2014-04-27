using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Disqus.Data
{
    public class Author
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("about")]
        public string About { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("isAnonymous")]
        public bool IsAnonymous { get; set; }
        
        //rep

        [JsonProperty("profileUrl")]
        public string ProfileUrl { get; set; }

        [JsonProperty("reputation")]
        public double Reputation { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("isPrivate")]
        public bool IsPrivate { get; set; }

        [JsonProperty("isPrimary")]
        public bool IsPrimary { get; set; }

        [JsonProperty("joinedAt")]
        public DateTime JoinedAt { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("avatar")]
        public Avatar Avatar { get; set; }
    }
}
