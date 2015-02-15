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
        private ICommentsService _commentsService;
        private IApplicationSettingsService _applicationSettingsService;
        private readonly Dictionary<string, List<Article>> _articles;
         private readonly Dictionary<string, int> _nextPages;
        public BlogRepository(
            IHttpClientService httpClientService, 
            IConfigurationService configurationService, 
            ICommentsService commentsService,
            IApplicationSettingsService applicationSettingsService)
        {
            _httpClientService = httpClientService;
            _configurationService = configurationService;
            _commentsService = commentsService;
            _applicationSettingsService = applicationSettingsService;
            _articles = new Dictionary<string, List<Article>>();
            _nextPages = new Dictionary<string, int>();
        }
        public async Task<RepositoryResult<Article[]>> GetArticlesAsync(string category, bool update, CancellationToken cancellationToken)
        {
            try
            {
                var feedUrl = _configurationService.GetFeedUrl(category);
                if (!_articles.ContainsKey(category))
                {
                    _articles.Add(category, new List<Article>());
                    _nextPages.Add(category, 1);
                }
                if (
                    //update || 
                    _articles[category].Count == 0)
                {
                    _articles[category].Clear();
                    _nextPages[category] = 1;
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
            var feed = await _httpClientService.GetXmlAsync<RssFeed>(feedUrl + "?paged=" + _nextPages[category], cancellationToken);
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

            _nextPages[category]++;
            return cancellationToken;
        }

        public async Task<RepositoryResult<Comment[]>> GetCommentsAsync(Article article, CancellationToken cancellationToken)
        {
            try
            {
                return await _commentsService.GetCommentsAsync(article, cancellationToken);
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

                    while (extractedContent.Contains("<div class=\"tiled-gallery "))
                    {
                        extractedContent = fixGallery(extractedContent);
                    }

                    var match = Regex.Match(extractedContent, "http:\\\\/\\\\/static.polldaddy.com\\\\/p\\\\/(\\d+).js", RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        extractedContent += "<script type='text/javascript' charset='UTF-8' src='http://i0.poll.fm/js/rating/rating.js'></script>";
                        extractedContent += string.Format(
                            "<script type='text/javascript' charset='UTF-8' src='http://static.polldaddy.com/p/{0}.js'></script>",
                            match.Groups[1].Value
                            );
                    }

                    //var matchesYouTubeList = Regex.Matches(extractedContent,
                    //    "<span class='embed-youtube'.*?\\swidth='(\\d+)'.*?\\sheight='(\\d+)'.*?src='http://www.youtube.com/embed/videoseries\\?list=(\\w+).*'.*?</span>", RegexOptions.IgnoreCase);

                    //if (matchesYouTubeList.Count > 0)
                    //{
                    //    foreach (Match item in matchesYouTubeList)
                    //    {
                    //        extractedContent = extractedContent.Replace(item.Value,
                    //            string.Format(
                    //                "<div class='embed-youtube' style='position:relative;'><a href='#' onclick='window.external.Notify(\"youtube:{0}\");return false;'><div class='_nmyt' ></div><div><img src='http://img.youtube.com/vi/{0}/hqdefault.jpg'></img></div></a></div>",
                    //                item.Groups[3]
                    //                ));
                    //    }
                    //}

                    var matchesYouTube = Regex.Matches(extractedContent,
                        "<span class='embed-youtube'.*?\\swidth='(\\d+)'.*?\\sheight='(\\d+)'.*?src='http://www.youtube.com/embed/([\\w_\\-]+).*'.*?</span>",RegexOptions.IgnoreCase);

                    if (matchesYouTube.Count >  0)
                    {
                        foreach (Match item in matchesYouTube)
                        {
                            if (item.Value.Contains("embed/videoseries"))
                                continue;
                            //extractedContent = extractedContent.Replace(item.Value,
                            //    string.Format(
                            //        "<div class='embed-youtube' style='position:relative;'><a href='#' onclick='window.external.Notify(\"youtube:{0}\");return false;'><div class='_nmyt' ></div><div><img src='http://img.youtube.com/vi/{0}/hqdefault.jpg'></img></div></a></div>",
                            //        item.Groups[3]
                            //        ));
                            var original = item.Value;

                            var modified = original
                                .Replace("span", "div")
                                .Replace("<iframe","<div><iframe")
                                .Replace("</iframe>", string.Format("</iframe><div onclick='window.external.Notify(\"youtube:{0}\");return false;'></div></div>", item.Groups[3]));

                            extractedContent = extractedContent.Replace(original, modified);
                        }
                    }

                    var matches = Regex.Matches(extractedContent, "href=\"" + baseArticleUrl + "(\\d*)" + "/?\"", RegexOptions.IgnoreCase);
                    if(matches.Count>0)
                    {
                        foreach (Match item in matches)
                        {
                            var pageNumber = item.Groups[1].Value;
                            if (string.IsNullOrEmpty(pageNumber))
                                pageNumber = "1";
                            //extractedContent = extractedContent.Replace(item.Value, string.Format("javascript:void(window.external.Notify('reload:{0}'));",item.Value));
                            extractedContent = extractedContent.Replace(item.Value, string.Format("href=\"#\" onclick=\"window.external.Notify('reload:{0}{1}/');return false;\"", baseArticleUrl, pageNumber));

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

        private string fixGallery(string extractedContent)
        {
            var start = extractedContent.IndexOf("<div class=\"tiled-gallery ");
            var end = extractedContent.IndexOf("<!-- close row --> </div>", start) + "<!-- close row --> </div>".Length;
            var sub = extractedContent.Substring(start, end - start);
            var imgStart = sub.IndexOf("data-orig-file=\"") + "data-orig-file=\"".Length;
            var imgEnd = sub.IndexOf("\"", imgStart);
            var imgUrl = sub.Substring(imgStart, imgEnd - imgStart);
            return extractedContent.Replace(sub, string.Format("<p><img src=\"{0}\"/></p>", imgUrl));
        }

        public async Task<RepositoryResult<CommentsInfo>> GetCommentsInfoAsync(string articleUrl, CancellationToken cancellationToken)
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
                var commentsInfo = await _commentsService.GetCommentsInfoAsync(article, cancellationToken);
                if (commentsInfo != null)
                {
                    if (commentsInfo.Successful && article != null)
                    {
                        article.Id = commentsInfo.Value.Id;
                        article.CommentsCount = commentsInfo.Value.Count;
                    }
                    return commentsInfo;
                }
                return RepositoryResult<CommentsInfo>.CreateError(new NotImplementedException());
            }
            catch (Exception xcp)
            {
                return RepositoryResult<CommentsInfo>.CreateError(xcp);
            }
        }

        public async Task<RepositoryResult<Comment>> CreateCommentAsync(Article article, string message, string parent, CancellationToken cancellationToken)
        {
            try
            {
                var guestUser = _applicationSettingsService.GetGuestUserAccount();
                return await _commentsService.CreateCommentAsync(article, guestUser.UserName, guestUser.Email, message, parent, cancellationToken);
            }
            catch (Exception xcp)
            {
                return RepositoryResult<Comment>.CreateError(xcp);
            }
        }


        public RepositoryResult<bool> IsGuestAccountValid()
        {
            try
            {
                return _applicationSettingsService.GetGuestUserAccount().IsValid;

            }
            catch (Exception xcp)
            {
                return RepositoryResult<bool>.CreateError(xcp);
            }
        }
    }
}
