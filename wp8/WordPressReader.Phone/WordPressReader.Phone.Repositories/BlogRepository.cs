using MSC.Phone.Shared.Contracts.Models;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WordPressReader.Phone.Contracts.Models;
using WordPressReader.Phone.Contracts.Repositories;
using System.Linq;

namespace WordPressReader.Phone.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private IHttpClientService _httpClientService;
        private readonly List<Article> _articles;

        public BlogRepository(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _articles = new List<Article>();
        }
        public async Task<Article[]> GetArticlesAsync(CancellationToken cancellationToken)
        {
            var feedUrl = "http://www.vitkigurman.com/feed";
            if(_articles.Count == 0)
            {
                var feed = await _httpClientService.GetXmlAsync<RssFeed>(feedUrl, cancellationToken);
                _articles.AddRange(
                    feed.Channel.Items.Select(
                    item => new Article {
                        Title = item.Title, 
                        Description = item.Description,
                        Link = item.Link,
                        PublishingDate = item.Date, 
                        CommentLink = item.CommentRss })
                    );
            }
            return _articles.ToArray();
        }

        public async Task<string> GetArticleContentAsync(string articleUrl, CancellationToken cancellationToken)
        {
            var article = _articles.FirstOrDefault(a => a.Link == articleUrl);
            if(article == null)
            {
                return null;
            }
            if(string.IsNullOrEmpty(article.Content))
            {
                article.Content = await _httpClientService.GetRawAsync(articleUrl, cancellationToken);
            }
            return article.Content;
        }
    }
}
