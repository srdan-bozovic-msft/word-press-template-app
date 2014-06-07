using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Disqus.Data
{
    public class ForumThread
    {
        [JsonProperty("feed")]
        public string Feed { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("forum")]
        public string Forum { get; set; }

        [JsonProperty("clean_title")]
        public string CleanTitle { get; set; }

        //public Post HighlightedPost { get; set; }

        [JsonProperty("userScore")]
        public int UserScore { get; set; }

        [JsonProperty("identifiers")]
        public string[] Identifiers { get; set; }

        [JsonProperty("dislikes")]
        public int Dislikes { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("isClosed")]
        public bool IsClosed { get; set; }

        [JsonProperty("posts")]
        public int Posts { get; set; }

        [JsonProperty("userSubscription")]
        public bool UserSubscription { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
