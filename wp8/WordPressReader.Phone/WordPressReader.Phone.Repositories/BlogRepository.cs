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
using WordPressReader.Phone.Contracts.Services;

namespace WordPressReader.Phone.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private IHttpClientService _httpClientService;
        private IConfigurationService _configurationService;
        private readonly List<Article> _articles;
        private readonly List<Comment> _comments;
        public BlogRepository(IHttpClientService httpClientService, IConfigurationService configurationService)
        {
            _httpClientService = httpClientService;
            _configurationService = configurationService;
            _articles = new List<Article>();
            _comments = new List<Comment>();
        }
        public async Task<Article[]> GetArticlesAsync(bool update, CancellationToken cancellationToken)
        {
            var feedUrl = _configurationService.GetFeedUrl();
            if (update || _articles.Count == 0)
            {
                _articles.Clear();
                var fetch = true;
                var page = 1;
                while (fetch)
                {
                    var feed = await _httpClientService.GetXmlAsync<RssFeed>(feedUrl+"?paged="+page, cancellationToken);
                    _articles.AddRange(
                        feed.Channel.Items
                        .Where(i => string.Join("", i.Categories).Contains(_configurationService.GetCategoryFilter()))
                        .Select(
                        item => new Article
                        {
                            Title = item.Title,
                            Description = item.Description,
                            Link = item.Link,
                            PublishingDate = item.Date,
                            CommentLink = item.CommentRss,
                            Category = item.Categories.FirstOrDefault()
                        }));
                    page++;
                    if (feed.Channel.Items.Length == 0)
                        fetch = false;
                    else if (feed.Channel.Items.Last().Date < DateTime.Now.AddMonths(-3))
                        fetch = false;
                }
            }
            return _articles.ToArray();
        }

        public async Task<Comment[]> GetCommentsAsync(string url, CancellationToken cancellationToken)
        {
            _comments.Clear();
            var feed = await _httpClientService.GetXmlAsync<RssFeed>(url, cancellationToken);
            _comments.AddRange(
                feed.Channel.Items.Select(
                item => new Comment
                {
                    Content = item.Description,
                })
            );
            return _comments.ToArray();
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
                article.Content =
                    string.Format(await _configurationService.GetArticleTemplateAsync(),
                        HtmlHelper.ExtractContent(
                            await _httpClientService.GetRawAsync(articleUrl, cancellationToken),
                            _configurationService.GetContentXPath())
                    );
            }
            return article.Content;
        }
    }
}
