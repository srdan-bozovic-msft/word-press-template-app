using MSC.Phone.Disqus.Data;
using MSC.Phone.Shared.Contracts.Services;
using System;
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

namespace MSC.Phone.Disqus
{
    public class DisqusClient
    {
        private const string BaseUrl = "https://disqus.com/api/3.0";

        private readonly string _apiKey;
        private readonly IHttpClientService _httpClientService;

        public DisqusClient(string apiKey, IHttpClientService httpClientService)
        {
            _apiKey = apiKey;
            _httpClientService = httpClientService;
        }

        public async Task<Page<Post>> GetPostsForThreadUrlAsync(CancellationToken cancellationToken, string forum, string threadUrl, string cursor = null, int? limit = null)
        {
            var url = string.Format("{0}/posts/list.json?forum={1}&thread=link:{2}&api_key={3}",
                BaseUrl,
                forum,
                threadUrl,
                _apiKey
                );
            if (cursor != null)
                url += "&cursor=" + cursor;
            if (limit != null)
                url += "&limit=" + limit;
            return await _httpClientService.GetJsonAsync<Page<Post>>(url, cancellationToken);
        }

        public async Task<Page<ForumThread>> GetThreadAsync(CancellationToken cancellationToken, string forum, string threadUrl, string cursor = null, int? limit = null)
        {
            var url = string.Format("{0}/threads/list.json?forum={1}&thread=link:{2}&api_key={3}",
                BaseUrl,
                forum,
                threadUrl,
                _apiKey
                );
            if (cursor != null)
                url += "&cursor=" + cursor;
            if (limit != null)
                url += "&limit=" + limit;
            return await _httpClientService.GetJsonAsync<Page<ForumThread>>(url, cancellationToken);
        }

    }
}
