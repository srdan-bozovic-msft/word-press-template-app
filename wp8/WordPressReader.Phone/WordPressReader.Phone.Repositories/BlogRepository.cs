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
using MSC.Phone.Shared.Contracts.Repositories;
using System.Text.RegularExpressions;

namespace WordPressReader.Phone.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private IHttpClientService _httpClientService;
        private IConfigurationService _configurationService;
        private readonly Dictionary<string, List<Article>> _articles;
        private readonly List<Comment> _comments;
        private int _nextPage = 0;
        public BlogRepository(IHttpClientService httpClientService, IConfigurationService configurationService)
        {
            _httpClientService = httpClientService;
            _configurationService = configurationService;
            _articles = new Dictionary<string, List<Article>>();
            _comments = new List<Comment>();
        }
        public async Task<RepositoryResult<Article[]>> GetArticlesAsync(string category, bool update, CancellationToken cancellationToken)
        {
            try
            {
                var feedUrl = _configurationService.GetFeedUrl(category);
                if (!_articles.ContainsKey(category))
                    _articles.Add(category, new List<Article>());
                if (update || _articles[category].Count == 0)
                {
                    _articles[category].Clear();
                    _nextPage = 1;
                    await FetchAsync(category, cancellationToken, feedUrl);
                }
                return _articles[category].ToArray();
            }
            catch (Exception xcp)
            {
                return RepositoryResult<Article[]>.CreateError(xcp);
            }
        }

        public async Task<RepositoryResult<Article[]>> GetMoreArticlesAsync(string category, CancellationToken cancellationToken)
        {
            try
            {
                var feedUrl = _configurationService.GetFeedUrl(category);
                await FetchAsync(category, cancellationToken, feedUrl);
                return _articles[category].ToArray();
            }
            catch (Exception xcp)
            {
                return RepositoryResult<Article[]>.CreateError(xcp);
            }
        }

        //private async Task<CancellationToken> FetchAsync(CancellationToken cancellationToken, string feedUrl)
        //{
        //    var fetch = true;
        //    while (fetch)
        //    {
        //        var feed = await _httpClientService.GetXmlAsync<RssFeed>(feedUrl + "?paged=" + _nextPage, cancellationToken);
        //        var categoryFilter = _configurationService.GetCategoryFilter();

        //        var items = feed.Channel.Items;

        //        if (!string.IsNullOrEmpty(categoryFilter))
        //        {
        //            items = items.Where(i => string.Join("", i.Categories).Contains(_configurationService.GetCategoryFilter())).ToArray();
        //        }

        //        _articles.AddRange(
        //            items.Select(
        //            item => new Article
        //            {
        //                Title = item.Title,
        //                Description = item.Description,
        //                Link = item.Link,
        //                PublishingDate = item.Date,
        //                CommentLink = item.CommentRss,
        //                Category = item.Categories.FirstOrDefault()
        //            }));

        //        _nextPage++;
        //        if (_articles.Count > 10)
        //            fetch = false;
        //        else if (feed.Channel.Items.Length == 0)
        //            fetch = false;
        //        else if (feed.Channel.Items.Last().Date < DateTime.Now.AddMonths(-3))
        //            fetch = false;
        //    }
        //    return cancellationToken;
        //}

        private async Task<CancellationToken> FetchAsync(string category, CancellationToken cancellationToken, string feedUrl)
        {
            var feed = await _httpClientService.GetXmlAsync<RssFeed>(feedUrl + "?paged=" + _nextPage, cancellationToken);
            var categoryFilter = _configurationService.GetCategoryFilter();

            var items = feed.Channel.Items;

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                items = items.Where(i => string.Join("", i.Categories).Contains(_configurationService.GetCategoryFilter())).ToArray();
            }

            _articles[category].AddRange(
                items.Select(
                item => new Article
                {
                    Title = item.Title,
                    Description = item.Description,
                    EncodedContent = item.EncodedContent,
                    Link = item.Link,
                    PublishingDate = item.Date,
                    Creator = item.Creator,
                    CommentLink = item.CommentRss,
                    Category = item.Categories.FirstOrDefault()
                }));

            _nextPage++;
            return cancellationToken;
        }

        public async Task<RepositoryResult<Comment[]>> GetCommentsAsync(string url, CancellationToken cancellationToken)
        {
            try
            {
                _comments.Clear();
                var feed = await _httpClientService.GetXmlAsync<RssFeed>(url, cancellationToken);
                if (feed != null && feed.Channel.Items != null)
                {
                    _comments.AddRange(
                        feed.Channel.Items.Select(
                        item => new Comment
                        {
                            Content = item.Description,
                        })
                    );
                }
                return _comments.ToArray();
            }
            catch (Exception xcp)
            {
                return RepositoryResult<Comment[]>.CreateError(xcp);
            }
        }

        public async Task<RepositoryResult<string>> GetArticleContentAsync(string articleUrl, CancellationToken cancellationToken)
        {
            try
            {
                Article article = null;
                foreach (var category in _articles.Keys)
                {
                    article = _articles[category].FirstOrDefault(a => a.Link == articleUrl);
                    if (article != null)
                        break;
                }
                if (article ==null || string.IsNullOrEmpty(article.Content))
                {
                    var baseArticleUrl = articleUrl;
                    if(article==null)
                    {
                        baseArticleUrl = baseArticleUrl.TrimEnd('/');
                        baseArticleUrl = baseArticleUrl.Remove(baseArticleUrl.LastIndexOf('/') + 1);
                    }
                    var extractedContent = HtmlHelper.ExtractContent(
                                await _httpClientService.GetRawAsync(articleUrl, cancellationToken).ConfigureAwait(false),
                                _configurationService.GetContentXPath());

                    var matches = Regex.Matches(extractedContent, "href=\"" + baseArticleUrl + "(\\d+)" + "/\"", RegexOptions.IgnoreCase);
                    if(matches.Count>0)
                    {
                        foreach (Match item in matches)
                        {
                            //extractedContent = extractedContent.Replace(item.Value, string.Format("javascript:void(window.external.Notify('reload:{0}'));",item.Value));
                            extractedContent = extractedContent.Replace(item.Value, string.Format("href=\"#\" onclick=\"window.external.Notify('reload:{0}{1}/');return false;\"", baseArticleUrl, item.Groups[1].Value));

                        } 
                    }

                    var content = string.Format(await _configurationService.GetArticleTemplateAsync(),
                            _configurationService.ProcessHtml(extractedContent));

                    if(article==null)
                        return content;

                    article.Content = content;
                }
                return article.Content;
            }
            catch(Exception xcp)
            {
                return RepositoryResult<string>.CreateError(xcp);
            }
        }
    }
}
