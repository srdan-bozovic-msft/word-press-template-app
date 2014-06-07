using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WordPressReader.Phone.Contracts.Models
{
    public class Comment
    {
        public string Id { get; set; }

        public string Parent { get; set; }

        public string Author { get; set; }
        
        public string AuthorAvatarUrl { get; set; }

        public bool IsAuthorAnonymous { get; set; }

        public int Level { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsHighlighted { get; set; }

        private string _content;

        public string RawContent
        {
            get
            {
                return _content;
            }
        }

        public string Content
        {
            get
            {
                return HttpUtility.HtmlDecode(_content);
            }
            set
            {
                _content = value;
            }
        }
    }
}
