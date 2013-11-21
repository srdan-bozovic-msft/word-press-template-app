using GalaSoft.MvvmLight;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WordPressReader.Phone.Contracts.ViewModels;

namespace WordPressReader.Phone.ViewModels
{
    public class ArticlePageViewModel : ViewModelBase, IArticlePageViewModel
    {
        private IHttpClientService _httpClientService;

        private string _pageTitle;
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                _pageTitle = value;
                RaisePropertyChanged(() => PageTitle);
            }
        }

        private string _html;
        public  string Html
        {
            get 
            {
                return _html;
            }
            set
            {
                _html = value;
                RaisePropertyChanged(() => Html);
            }
        }

        public ArticlePageViewModel(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            PageTitle = "Vitki Gurman";
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            var cts = new CancellationTokenSource();
            Html = await _httpClientService.GetRawAsync("http://www.vitkigurman.com/2013/11/16/coca-cola-sudar-svetova/", cts.Token);
        }
    }
}
