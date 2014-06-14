using MSC.Phone.Disqus;
using MSC.Phone.Disqus.Data;
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
            var comments = new List<Comment>();
            var orphenComments = new Dictionary<string, List<Comment>>();
            foreach (var post in posts.Response)
            {
                var comment = PostToComment(post);
                if(!string.IsNullOrEmpty(comment.Parent))
                {
                    if(!orphenComments.ContainsKey(comment.Parent))
                    {
                        orphenComments.Add(comment.Parent, new List<Comment>());
                    }
                    orphenComments[comment.Parent].Add(comment);
                }
                else
                {
                    AddComment(comments, orphenComments, comment);
                }
            }
            return comments.ToArray();
        }

        private static void AddComment(List<Comment> comments, Dictionary<string, List<Comment>> orphenComments, Comment comment)
        {
            comments.Add(comment);
            if(orphenComments.ContainsKey(comment.Id))
            {
                foreach (var orphenComment in orphenComments[comment.Id])
                {
                    orphenComment.Level = comment.Level + 1;
                    AddComment(comments, orphenComments, orphenComment);
                }
            }
        }

        private static Comment PostToComment(Post post)
        {
            return new Comment
            {
                Id = post.Id,
                Author = post.Author.Name,
                AuthorAvatarUrl = post.Author.Avatar.Small.Permalink,
                IsAuthorAnonymous = post.Author.IsAnonymous,
                Content = post.RawMessage,
                CreatedAt = post.CreatedAt,
                Dislikes = post.Dislikes,
                IsHighlighted = post.IsHighlighted,
                Likes = post.Likes,
                Parent = post.Parent
            };
        }

        public async Task<int?> GetCommentsCountAsync(Article article, CancellationToken cancellationToken)
        {
            var threads = await _client.GetThreadAsync(
                cancellationToken,
                "nokiamob",
                article.Link);
            if (threads.Response.Count() == 0)
                return null;
            return threads.Response[0].Posts;
        }


        public async Task<ServiceResult<Comment>> CreateCommentAsync(Article article, string authorName, string authorEmail, string message, string parent, CancellationToken cancellationToken)
        {
            var result = await _client.CreatePostAsync(cancellationToken, article.Id, authorEmail, authorEmail, message, parent);
            if(result.IsSuccessful)
            {
                return PostToComment(result.Response);
            }
            else
            {
                return ServiceResult<Comment>.Create(null, false, result.Code, result.ErrorMessage);
            }
        }
    }
}
