using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using MSC.Phone.Shared.Contracts.PhoneServices;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WordPressReader.Phone.Contracts.Models;
using WordPressReader.Phone.Contracts.Repositories;
using WordPressReader.Phone.Contracts.ViewModels;

namespace WordPressReader.Phone.ViewModels
{
    public class ArticleListPageViewModel : ViewModelBase, IArticlePageViewModel
    {
        private readonly IBlogRepository _blogRepository;
        private readonly INavigationService _navigationService;
        private readonly ISocialShare _socialShare;
        private Article[] _articles;

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

        public ArticleViewModel Current
        {
            get
            {
                var currentLink = _articles[_current].Link;
                if (currentLink == ArticleOne.Link)
                    return ArticleOne;
                if (currentLink == ArticleTwo.Link)
                    return ArticleTwo;
                if (currentLink == ArticleThree.Link)
                    return ArticleThree;
                return null;
            }
        }

        private ArticleViewModel _articleOne;
        public ArticleViewModel ArticleOne
        {
            get
            {
                return _articleOne;
            }
            set
            {
                _articleOne = value;
                RaisePropertyChanged(() => ArticleOne);
            }
        }

        private ArticleViewModel _articleTwo;
        public ArticleViewModel ArticleTwo
        {
            get
            {
                return _articleTwo;
            }
            set
            {
                _articleTwo = value;
                RaisePropertyChanged(() => ArticleTwo);
            }
        }

        private ArticleViewModel _articleThree;
        public ArticleViewModel ArticleThree
        {
            get
            {
                return _articleThree;
            }
            set
            {
                _articleThree = value;
                RaisePropertyChanged(() => ArticleThree);
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

        public ArticleListPageViewModel(IBlogRepository blogRepository, INavigationService navigationService, ISocialShare socialShare)
        {
            _blogRepository = blogRepository;
            _navigationService = navigationService;
            _socialShare = socialShare;
            
            FlipArticleHorizontalCommand = new RelayCommand<double>(async velocity => {
                try
                {
                    var delta = velocity > 0 ? -1 : 1;
                    var oldValue = SelectedIndex;
                    SelectedIndex += delta;
                    _current = (_current + delta + _articles.Length) % _articles.Length;
                    await OnSetIndex(SelectedIndex, oldValue);
                }
                catch(Exception)
                {
                    _navigationService.Navigate("Error");
                }
            });

            ArticleOne = new ArticleViewModel(blogRepository, navigationService, socialShare)
            {
                FlipArticleHorizontalCommand = FlipArticleHorizontalCommand
            };
            ArticleTwo = new ArticleViewModel(blogRepository, navigationService, socialShare)
            {
                FlipArticleHorizontalCommand = FlipArticleHorizontalCommand
            };
            ArticleThree = new ArticleViewModel(blogRepository, navigationService, socialShare)
            {
                FlipArticleHorizontalCommand = FlipArticleHorizontalCommand
            };

            GoToCommentsCommand = new RelayCommand(
                () => Current.GoToCommentsCommand.Execute(null));
            
            ShareCommand = new RelayCommand(
                () => Current.ShareCommand.Execute(null));
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            var parameters = ((string)parameter).Split(new[] { ";;" }, 2, StringSplitOptions.RemoveEmptyEntries);
            ArticleOne.Category = ArticleTwo.Category = ArticleThree.Category = parameters[0];

            var cts = new CancellationTokenSource();
            _articles = await _blogRepository.GetArticlesAsync(parameters[0], false, cts.Token);
            _current = -1;
            for (int i = 0; i < _articles.Length; i++)
            {
                if (_articles[i].Link == parameters[1])
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
            using (TimedAnalyticsEvent token = ClientAnalyticsChannel.Default.StartTimedEvent("Phone/Article/SetPage"))
            {
                try
                {
                    var count = _articles.Length;
                    var article = _articles[(_current + count) % count];
                    ClientAnalyticsChannel.Default.LogPageView("Phone/Article");
                    var cts = new CancellationTokenSource();
                    if (newValue == 1)
                    {
                        await ArticleTwo.LoadAsync(article, cts.Token);
                    }
                    if (newValue == 2)
                    {
                        await ArticleThree.LoadAsync(article, cts.Token);
                    }
                    if (newValue == 0)
                    {
                        await ArticleOne.LoadAsync(article, cts.Token);
                    }
                }
                catch (Exception)
                {
                    token.Cancel();
                    _navigationService.Navigate("Error");
                }
            }
        }

        public ICommand FlipArticleHorizontalCommand { get; set; }

        public ICommand GoToCommentsCommand { get; set; }

        public ICommand ShareCommand { get; set; }

        public ICommand ChangePageCommand { get; set; }
    }
}
