using MSC.Phone.Disqus.Data;
using MSC.Phone.Shared.Contracts.Services;
using Newtonsoft.Json;
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

        public async Task<PageResult<Post>> GetPostsForThreadUrlAsync(CancellationToken cancellationToken, string forum, string threadUrl, string cursor = null, int? limit = null)
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
            return await _httpClientService.GetJsonAsync<PageResult<Post>>(url, cancellationToken);
        }

        public async Task<PageResult<ForumThread>> GetThreadAsync(CancellationToken cancellationToken, string forum, string threadUrl, string cursor = null, int? limit = null)
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
            return await _httpClientService.GetJsonAsync<PageResult<ForumThread>>(url, cancellationToken);
        }

        public async Task<Result<Post>> CreatePostAsync(CancellationToken cancellationToken, string thread, string authorName, string authorEmail, string message, string parent = null)
        {
            var url = string.Format("{0}/posts/create.json", BaseUrl);

            var headers = new KeyValuePair<string, string>[]{
                    new KeyValuePair<string,string>("Accept","*/*"),
                    new KeyValuePair<string,string>("Origin","http://disqus.com"),
                    new KeyValuePair<string,string>("X-Requested-With","XMLHttpRequest"),
                };

            var parameters = new List<KeyValuePair<string, string>>{
                    new KeyValuePair<string,string>("thread", thread),
                    new KeyValuePair<string,string>("author_name", authorName),
                    new KeyValuePair<string,string>("author_email", authorEmail),     
                    new KeyValuePair<string,string>("message", message),
                    new KeyValuePair<string,string>("api_key", _apiKey),
                };

            if (!string.IsNullOrEmpty(parent))
                parameters.Add(new KeyValuePair<string, string>("parent", parent));
            
            var response = await _httpClientService.CallAsync(
                "POST",
                url,
                headers,
                parameters,
                cancellationToken);

            if (response.Item1 == HttpStatusCode.BadRequest)
            {
                var json = JsonConvert.DeserializeObject<ErrorResult>(response.Item2);
                return new Result<Post> { Code = json.Code, ErrorMessage = json.Response };
            }
            else
            {
                return JsonConvert.DeserializeObject<Result<Post>>(response.Item2);
            }
        }
    }
}
