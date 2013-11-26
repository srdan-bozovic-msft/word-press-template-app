using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPressReader.Phone.Repositories
{
    public static class HtmlHelper
    {
        public static string ExtractContent(string html, string xpath)
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(html);
            var node = htmlDoc.DocumentNode.SelectSingleNode(xpath);
            return node.InnerHtml;
        }

    }
}
