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
        private readonly string _apiKey;
        private readonly IHttpClientService _httpClientService;

        public DisqusClient(string apiKey, IHttpClientService httpClientService)
        {
            _apiKey = apiKey;
            _httpClientService = httpClientService;
        }

        public async Task<Page<Post>> GetPostsForThreadUrlAsync(string threadUrl, CancellationToken cancellationToken)
        {
            var url = "";
            return await _httpClientService.GetJsonAsync<Page<Post>>(url, cancellationToken);
        }

    }
}
