using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPressReader.Phone.Contracts.Models
{
    public class Article
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishingDate { get; set; }
        public string Link { get; set; }
        public string CommentLink { get; set; }
        public string Content { get; set; }

        public string Category { get; set; }
    }
}
