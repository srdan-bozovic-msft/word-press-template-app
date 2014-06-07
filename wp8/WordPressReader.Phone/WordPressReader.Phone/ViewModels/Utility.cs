using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WordPressReader.Phone.ViewModels
{
    static class Utility
    {
        public static string HtmlDecode(string text)
        {
            return HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(text)).Trim();
        }
    }
}
