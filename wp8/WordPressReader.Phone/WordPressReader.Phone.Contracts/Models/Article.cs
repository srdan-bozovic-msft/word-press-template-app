using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordPressReader.Phone.Contracts.Models
{
    public class Article
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public DateTime PublishingDate { get; set; }
        public string Link { get; set; }
        public string CommentLink { get; set; }
        public int? CommentsCount { get; set; }
        public string EncodedContent { get; set; }

        public string ImageUrl
        {
            get
            {
                var matchString = Regex.Match(EncodedContent, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                return matchString;
            }
        }


        public string Content { get; set; }
        public string Category { get; set; }
    }
}
