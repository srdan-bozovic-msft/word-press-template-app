using MSC.Phone.Shared.Contracts.Models;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WordPressReader.Phone.Contracts.Models;
using WordPressReader.Phone.Contracts.Services;

namespace WordPressReader.Phone.Services
{
    public class RssCommentsService : ICommentsService
    {
        private IHttpClientService _httpClientService;
        public RssCommentsService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<Comment[]> GetCommentsAsync(Article article, CancellationToken cancellationToken)
        {
            var feed = await _httpClientService.GetXmlAsync<RssFeed>(article.CommentLink, cancellationToken);
            if (feed != null && feed.Channel.Items != null)
            {
                return
                    feed.Channel.Items.Select(
                    item => new Comment
                    {
                        Content = item.Description,
                    }).ToArray();
            }
            return new Comment[0];  
        }

        public async Task<int?> GetCommentsCountAsync(Article article, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
