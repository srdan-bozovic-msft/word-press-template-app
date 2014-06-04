using MSC.Phone.Disqus;
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
    public class DisqusCommentsService : ICommentsService
    {
        private const string ApiKey = "E8Uh5l5fHZ6gD8U3KycjAIAk46f68Zw7C6eW8WSjZvCLXebZ7p0r1yrYDrLilk2F";
        private DisqusClient _client;
        public DisqusCommentsService(IHttpClientService httpClientService)
        {
            _client = new DisqusClient(ApiKey, httpClientService);
        }
        public async Task<Comment[]> GetCommentsAsync(Article article, CancellationToken cancellationToken)
        {
            var posts = await _client.GetPostsForThreadUrlAsync(
                cancellationToken,
                "nokiamob",
                article.Link,
                null,
                100);
            return posts.Response.Select(p => new Comment {
                Id = p.Id,
                Content = p.RawMessage,
                CreatedAt = p.CreatedAt,
                Dislikes = p.Dislikes,
                IsHighlighted = p.IsHighlighted,
                Likes = p.Likes,
                Parent = p.Parent
            }).ToArray();
        }
    }
}
