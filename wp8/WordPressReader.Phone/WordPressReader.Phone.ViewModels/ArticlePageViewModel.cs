using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MSC.Phone.Shared.Contracts.Models;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WordPressReader.Phone.Contracts.Models;
using WordPressReader.Phone.Contracts.Repositories;
using WordPressReader.Phone.Contracts.ViewModels;

namespace WordPressReader.Phone.ViewModels
{
    public class ArticlePageViewModel : ViewModelBase, IArticlePageViewModel
    {
        private IBlogRepository _blogRepository;
        private INavigationService _navigationService;
        private Article[] _articles;

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
        public  string HtmlOne
        {
            get 
            {
                return _html;
            }
            set
            {
                _html = value;
                RaisePropertyChanged(() => HtmlOne);
            }
        }

        private string _htmlNext;
        public string HtmlTwo
        {
            get
            {
                return _htmlNext;
            }
            set
            {
                _htmlNext = value;
                RaisePropertyChanged(() => HtmlTwo);
            }
        }

        private string _htmlPrevious;
        public  string HtmlThree
        {
            get 
            {
                return _htmlPrevious;
            }
            set
            {
                _htmlPrevious = value;
                RaisePropertyChanged(() => HtmlThree);
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = (value + 3) % 3;               
                RaisePropertyChanged(() => SelectedIndex);
            }
        }

        public ArticlePageViewModel(IBlogRepository blogRepository, INavigationService navigationService)
        {
            _blogRepository = blogRepository;
            _navigationService = navigationService;
            PageTitle = "Vitki Gurman";
            FlipArticleHorizontalCommand = new RelayCommand<double>(async velocity => {
                try
                {
                    var delta = velocity > 0 ? -1 : 1;
                    var oldValue = SelectedIndex;
                    SelectedIndex += delta;
                    _current = (_current + delta + _articles.Length) % _articles.Length;
                    await OnSetIndex(SelectedIndex, oldValue);
                }
                catch(Exception xcp)
                {

                }
            });
            GoToCommentsCommand = new RelayCommand(
                ()=>_navigationService.Navigate("Comments",_articles[_current].Link));
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            var cts = new CancellationTokenSource();
            _articles = await _blogRepository.GetArticlesAsync(cts.Token);
            _current = -1;
            for (int i = 0; i < _articles.Length; i++)
            {
                if(_articles[i].Link == parameter)
                {
                    _current = i;
                    break;
                }
            }
            await OnSetIndex(SelectedIndex, SelectedIndex);
        }

        private int _current;
        private async Task OnSetIndex(int newValue, int oldValue)
        {
            try
            {
                var count = _articles.Length;
                var url = _articles[(_current + count) % count].Link;
                var nextUrl = _articles[(_current + 1) % count].Link;
                var previousUrl = _articles[(_current - 1 + count) % count].Link;
                var cts = new CancellationTokenSource();
                if (newValue == 1)
                {
                    HtmlThree = "";
                    HtmlOne = "";
                    if(oldValue == 1)
                        HtmlTwo = await _blogRepository.GetArticleContentAsync(url, cts.Token);
                    var results = await Task.WhenAll(
                        _blogRepository.GetArticleContentAsync(nextUrl, cts.Token),
                        _blogRepository.GetArticleContentAsync(previousUrl, cts.Token));
                    HtmlThree = results[0];
                    HtmlOne = results[1];
                    return;
                }
                if (newValue == 2)
                {
                    HtmlTwo = "";
                    HtmlOne = "";
                    if (oldValue == 2)
                        HtmlThree = await _blogRepository.GetArticleContentAsync(url, cts.Token);
                    var results = await Task.WhenAll(
                        _blogRepository.GetArticleContentAsync(nextUrl, cts.Token),
                        _blogRepository.GetArticleContentAsync(previousUrl, cts.Token));
                    HtmlTwo = results[1];
                    HtmlOne = results[0];
                    return;
                }
                if (newValue == 0)
                {
                    HtmlThree = "";
                    HtmlTwo = "";
                    if (oldValue == 0)
                        HtmlOne = await _blogRepository.GetArticleContentAsync(url, cts.Token);
                    var results = await Task.WhenAll(
                        _blogRepository.GetArticleContentAsync(nextUrl, cts.Token),
                        _blogRepository.GetArticleContentAsync(previousUrl, cts.Token));
                    HtmlThree = results[1];
                    HtmlTwo = results[0];
                    return;
                }
            }
            catch(Exception xcp)
            {

            }
        }

        public ICommand FlipArticleHorizontalCommand { get; set; }

        public ICommand GoToCommentsCommand { get; set; }
    }
}
