using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MSC.Phone.Shared.Contracts.Models;
using MSC.Phone.Shared.Contracts.PhoneServices;
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
        private ISocialShare _socialShare;
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

        private string _htmlOne;
        public  string HtmlOne
        {
            get 
            {
                return _htmlOne;
            }
            set
            {
                _htmlOne = value;
                RaisePropertyChanged(() => HtmlOne);
            }
        }

        private string _titleOne;
        public string TitleOne
        {
            get
            {
                return _titleOne;
            }
            set
            {
                _titleOne = value;
                RaisePropertyChanged(() => TitleOne);
            }
        }

        private string _leadOne;
        public string LeadOne
        {
            get
            {
                return _leadOne;
            }
            set
            {
                _leadOne = value;
                RaisePropertyChanged(() => LeadOne);
            }
        }

        private string _positionOne;
        public string PositionOne
        {
            get
            {
                return _positionOne;
            }
            set
            {
                _positionOne = value;
                RaisePropertyChanged(() => PositionOne);
            }
        }

        private string _htmlTwo;
        public string HtmlTwo
        {
            get
            {
                return _htmlTwo;
            }
            set
            {
                _htmlTwo = value;
                RaisePropertyChanged(() => HtmlTwo);
            }
        }

        private string _titleTwo;
        public string TitleTwo
        {
            get
            {
                return _titleTwo;
            }
            set
            {
                _titleTwo = value;
                RaisePropertyChanged(() => TitleTwo);
            }
        }

        private string _leadTwo;
        public string LeadTwo
        {
            get
            {
                return _leadTwo;
            }
            set
            {
                _leadTwo = value;
                RaisePropertyChanged(() => LeadTwo);
            }
        }

        private string _positionTwo;
        public string PositionTwo
        {
            get
            {
                return _positionTwo;
            }
            set
            {
                _positionTwo = value;
                RaisePropertyChanged(() => PositionTwo);
            }
        }


        private string _htmlThree;
        public  string HtmlThree
        {
            get 
            {
                return _htmlThree;
            }
            set
            {
                _htmlThree = value;
                RaisePropertyChanged(() => HtmlThree);
            }
        }

        private string _titleThree;
        public string TitleThree
        {
            get
            {
                return _titleThree;
            }
            set
            {
                _titleThree = value;
                RaisePropertyChanged(() => TitleThree);
            }
        }

        private string _leadThree;
        public string LeadThree
        {
            get
            {
                return _leadThree;
            }
            set
            {
                _leadThree = value;
                RaisePropertyChanged(() => LeadThree);
            }
        }

        private string _positionThree;
        public string PositionThree
        {
            get
            {
                return _positionThree;
            }
            set
            {
                _positionThree = value;
                RaisePropertyChanged(() => PositionThree);
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

        public ArticlePageViewModel(IBlogRepository blogRepository, INavigationService navigationService, ISocialShare socialShare)
        {
            _blogRepository = blogRepository;
            _navigationService = navigationService;
            _socialShare = socialShare;
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
            ShareCommand = new RelayCommand(
                () => {
                    var article = _articles[_current];
                    _socialShare.ShareLink(article.Title, new Uri(article.Link, UriKind.RelativeOrAbsolute));
                });
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
                var article = _articles[(_current + count) % count];
                var nextArticle = _articles[(_current + 1 + count) % count];
                var previousArticle = _articles[(_current - 1 + count) % count];
                var url = article.Link;
                var nextUrl = _articles[(_current + 1) % count].Link;
                var previousUrl = _articles[(_current - 1 + count) % count].Link;
                var cts = new CancellationTokenSource();
                if (newValue == 1)
                {
                    HtmlThree = "";
                    HtmlOne = "";
                    TitleTwo = article.Title;
                    LeadTwo = string.Format("{0:00}.{1:00}.{2:0000} | {3}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category);
                    PositionTwo = string.Format("{0}/{1}", _current + 1, count);
                    if (oldValue == 1)
                    {
                        HtmlTwo = "";
                        HtmlTwo = await _blogRepository.GetArticleContentAsync(url, cts.Token);
                    }
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
                    TitleThree = article.Title;
                    LeadThree = string.Format("{0:00}.{1:00}.{2:0000} | {3}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category);
                    PositionThree = string.Format("{0}/{1}", _current + 1, count);
                    if (oldValue == 2)
                    {
                        HtmlThree = "";
                        HtmlThree = await _blogRepository.GetArticleContentAsync(url, cts.Token);
                    }
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
                    TitleOne = article.Title;
                    LeadOne = string.Format("{0:00}.{1:00}.{2:0000} | {3}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category);
                    PositionOne = string.Format("{0}/{1}", _current + 1, count);
                    if (oldValue == 0)
                    {
                        HtmlOne = "";
                        HtmlOne = await _blogRepository.GetArticleContentAsync(url, cts.Token);
                    }
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

        public ICommand ShareCommand { get; set; }
    }
}
