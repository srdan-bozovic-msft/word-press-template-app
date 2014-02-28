using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MSC.Phone.Shared.Contracts.Models;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class MainPageViewModel : ViewModelBase, IMainPageViewModel
    {
        private IBlogRepository _blogRepository;
        private INavigationService _navigationService;

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

        private readonly ObservableCollection<Article> _articles;
        public ObservableCollection<Article> Articles
        {
            get
            {
                return _articles;
            }
        }

        public MainPageViewModel(IBlogRepository blogRepository, INavigationService navigationService)
        {
            _blogRepository = blogRepository;
            _navigationService = navigationService;
            PageTitle = "Vitki Gurman";
            _articles = new ObservableCollection<Article>();
            SelectArticleCommand = new RelayCommand<Article>(
                article => 
                    _navigationService.Navigate("Article", article.Link)
                );
            ReloadCommand = new RelayCommand(async () =>
                {
                    await ReloadArticlesAsync();
                });
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            await ReloadArticlesAsync();
        }

        private async Task ReloadArticlesAsync()
        {
            IsLoading = true;
            var cts = new CancellationTokenSource();
            _articles.Clear();
            var articles = await _blogRepository.GetArticlesAsync(true, cts.Token);
            if (!articles.IsError)
            {
                foreach (var article in articles.Value)
                {
                    article.Title = Utility.HtmlDecode(article.Title);
                    article.Description = Utility.HtmlDecode(article.Description);
                    _articles.Add(article);
                }
            }
            else
            {
                _navigationService.Navigate("Error");
            }
            IsLoading = false;

        }

        public ICommand SelectArticleCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
    }
}
