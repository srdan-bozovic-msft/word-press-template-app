using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Disqus.Data
{
    public class Post
    {

        [JsonProperty("parent")]
        public string Parent { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonProperty("forum")]
        public string Forum { get; set; }

        [JsonProperty("thread")]
        public string Thread { get; set; }

        [JsonProperty("isApproved")]
        public bool IsApproved { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        //media

        [JsonProperty("isFlagged")]
        public bool IsFlagged { get; set; }

        [JsonProperty("dislikes")]
        public int Dislikes { get; set; }

        [JsonProperty("raw_message")]
        public string RawMessage { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("numReports")]
        public int NumReports { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("isEdited")]
        public bool IsEdited { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("isSpam")]
        public bool IsSpam { get; set; }

        [JsonProperty("isHighlighted")]
        public bool IsHighlighted { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }
    }
}
