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
using WordPressReader.Phone.Contracts.Services;
using WordPressReader.Phone.Contracts.ViewModels;

namespace WordPressReader.Phone.ViewModels
{
    public class ArticlePageViewModel : ViewModelBase, IArticlePageViewModel
    {
        private IBlogRepository _blogRepository;
        private INavigationService _navigationService;
        private ISocialShare _socialShare;
        private Article[] _articles;
        private string _category;

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

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
            PageTitle = "";
            FlipArticleHorizontalCommand = new RelayCommand<double>(async velocity => {
                IsLoading = true;
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
                    _navigationService.Navigate("Error");
                }
                IsLoading = false;
            });
            GoToCommentsCommand = new RelayCommand(
                () => _navigationService.Navigate("RichComments", _category + ";;" + _articles[_current].Link));
            ShareCommand = new RelayCommand(
                () => {
                    var article = _articles[_current];
                    _socialShare.ShareLink(article.Title, new Uri(article.Link, UriKind.RelativeOrAbsolute));
                });
            ChangePageCommand = new RelayCommand<string>(
                async p =>
                {
                    IsLoading = true;
                    var cts = new CancellationTokenSource();
                    var url = p.Split(new char[] { ':' }, 2)[1];
                    var content = await _blogRepository.GetArticleContentAsync(url, cts.Token);
                    switch (SelectedIndex)
                    {
                        case 0:
                            HtmlOne = content;
                            break;
                        case 1:
                            HtmlTwo = content;
                            break;
                        case 2:
                            HtmlThree = content;
                            break;
                        default:
                            break;
                    }
                    IsLoading = false;
                }
                );
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            var parameters = ((string)parameter).Split(new[] { ";;" }, 2, StringSplitOptions.RemoveEmptyEntries);
            _category = parameters[0];
            var cts = new CancellationTokenSource();
            _articles = await _blogRepository.GetArticlesAsync(_category, false, cts.Token);
            _current = -1;
            for (int i = 0; i < _articles.Length; i++)
            {
                if (_articles[i].Link == parameters[1])
                {
                    _current = i;
                    break;
                }
            }
            IsLoading = true;
            await OnSetIndex(SelectedIndex, SelectedIndex);
            IsLoading = false;
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
                    if (article.CommentsCount == null)
                        LeadTwo = string.Format("{0:00}.{1:00}.{2:0000} | {3}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category);
                    else
                        LeadTwo = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
                    PositionTwo = string.Format("{0}/{1}", _current + 1, count);
                    if (oldValue == 1)
                    {
                        HtmlTwo = "";
                        var htmlTask = _blogRepository.GetArticleContentAsync(url, cts.Token);
                        var commentsCountTask = _blogRepository.GetCommentsCountAsync(url, cts.Token);
                        var result = await htmlTask;
                        if (!result.Successful)
                            _navigationService.Navigate("Error");
                        HtmlTwo = result;
                        var commentResult = await commentsCountTask;
                        if (commentResult.Successful)
                            article.CommentsCount = commentResult.Value;
                        LeadTwo = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
                    }
                    else
                    {
                        var commentsCountTask = _blogRepository.GetCommentsCountAsync(url, cts.Token);
                        var commentResult = await commentsCountTask;
                        if (commentResult.Successful)
                            article.CommentsCount = commentResult.Value;
                        LeadTwo = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
                    }
                    var results = await Task.WhenAll(
                        _blogRepository.GetArticleContentAsync(nextUrl, cts.Token),
                        _blogRepository.GetArticleContentAsync(previousUrl, cts.Token));

                    if (!results[0].Successful || !results[1].Successful)
                        _navigationService.Navigate("Error");

                    HtmlThree = results[0];
                    HtmlOne = results[1];
                    return;
                }
                if (newValue == 2)
                {
                    HtmlTwo = "";
                    HtmlOne = "";
                    TitleThree = article.Title;
                    if (article.CommentsCount == null)
                        LeadThree = string.Format("{0:00}.{1:00}.{2:0000} | {3}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category);
                    else
                        LeadThree = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
                    PositionThree = string.Format("{0}/{1}", _current + 1, count);
                    if (oldValue == 2)
                    {
                        HtmlThree = "";
                        var htmlTask = _blogRepository.GetArticleContentAsync(url, cts.Token);
                        var commentsCountTask = _blogRepository.GetCommentsCountAsync(url, cts.Token);
                        var result = await htmlTask; 
                        if (!result.Successful)
                            _navigationService.Navigate("Error");
                        HtmlThree = result;
                        var commentResult = await commentsCountTask;
                        if (commentResult.Successful)
                            article.CommentsCount = commentResult.Value;
                        LeadThree = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
                    }
                    else
                    {
                        var commentsCountTask = _blogRepository.GetCommentsCountAsync(url, cts.Token);
                        var commentResult = await commentsCountTask;
                        if (commentResult.Successful)
                            article.CommentsCount = commentResult.Value;
                        LeadThree = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
                    }
                    var results = await Task.WhenAll(
                        _blogRepository.GetArticleContentAsync(nextUrl, cts.Token),
                        _blogRepository.GetArticleContentAsync(previousUrl, cts.Token));
                    if (!results[0].Successful || !results[1].Successful)
                        _navigationService.Navigate("Error");
                    HtmlTwo = results[1];
                    HtmlOne = results[0];
                    return;
                }
                if (newValue == 0)
                {
                    HtmlThree = "";
                    HtmlTwo = "";
                    TitleOne = article.Title;
                    if (article.CommentsCount == null)
                        LeadOne = string.Format("{0:00}.{1:00}.{2:0000} | {3}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category);
                    else
                        LeadOne = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
                    PositionOne = string.Format("{0}/{1}", _current + 1, count);
                    if (oldValue == 0)
                    {
                        HtmlOne = "";
                        var htmlTask = _blogRepository.GetArticleContentAsync(url, cts.Token);
                        var commentsCountTask = _blogRepository.GetCommentsCountAsync(url, cts.Token);
                        var result = await htmlTask; 
                        if (!result.Successful)
                            _navigationService.Navigate("Error");
                        HtmlOne = result;
                        var commentResult = await commentsCountTask;
                        if (commentResult.Successful)
                            article.CommentsCount = commentResult.Value;
                        LeadOne = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
                    }
                    else
                    {
                        var commentsCountTask = _blogRepository.GetCommentsCountAsync(url, cts.Token);
                        var commentResult = await commentsCountTask;
                        if (commentResult.Successful)
                            article.CommentsCount = commentResult.Value;
                        LeadOne = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
                    }

                    var results = await Task.WhenAll(
                        _blogRepository.GetArticleContentAsync(nextUrl, cts.Token),
                        _blogRepository.GetArticleContentAsync(previousUrl, cts.Token));
                    if (!results[0].Successful || !results[1].Successful)
                        _navigationService.Navigate("Error");
                    HtmlThree = results[1];
                    HtmlTwo = results[0];
                    return;
                }
            }
            catch(Exception xcp)
            {
                _navigationService.Navigate("Error");
            }
        }

        public ICommand FlipArticleHorizontalCommand { get; set; }

        public ICommand GoToCommentsCommand { get; set; }

        public ICommand ShareCommand { get; set; }

        public ICommand ChangePageCommand { get; set; }
    }
}
