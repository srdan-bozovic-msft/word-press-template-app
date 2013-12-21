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
        private string _content;
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
