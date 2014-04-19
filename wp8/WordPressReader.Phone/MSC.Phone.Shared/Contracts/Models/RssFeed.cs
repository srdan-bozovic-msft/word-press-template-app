using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MSC.Phone.Shared.Contracts.Models
{
    [XmlRoot(ElementName="rss")]
    public class RssFeed
    {
        public class RssFeedChannel
        {
            public class RssFeedItem
            {
                [XmlElement("title")]
                public string Title { get; set; }

                [XmlElement("description")]
                public string Description { get; set; }

                [XmlElement(ElementName="encoded", Namespace = "http://purl.org/rss/1.0/modules/content/")]
                public string EncodedContent { get; set; }

                [XmlElement("pubDate")]
                public string pubDate { get; set; }

                [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
                public string Creator { get; set; }

                [XmlElement("link")]
                public string Link { get; set; }

                [XmlElement(ElementName = "commentRss", Namespace = "http://wellformedweb.org/CommentAPI/")]
                public string CommentRss { get; set; }

                [XmlElement(ElementName = "category")]
                public string[] Categories { get; set; }

                [XmlIgnore]
                public DateTime Date
                {
                    get
                    {
                        DateTime _date;
                        DateTime.TryParse(pubDate, out _date);
                        return _date;
                    }
                }
            }
            
            [XmlElement(ElementName="item")]
            public RssFeedItem[] Items { get; set; }
        }

        [XmlElement(ElementName="channel")]
        public RssFeedChannel Channel { get; set; }

    }
}
