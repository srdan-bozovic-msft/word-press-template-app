using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MSC.Phone.Shared.Contracts.PhoneServices;
using MSC.Phone.Shared.Contracts.Services;
using MyToolkit.Multimedia;
using WordPressReader.Phone.Contracts.Models;
using WordPressReader.Phone.Contracts.Repositories;
using WordPressReader.Phone.Controls;

namespace WordPressReader.Phone.ViewModels
{
    public class ArticleViewModel : ViewModelBase
    {
        private readonly IBlogRepository _blogRepository;
        private readonly INavigationService _navigationService;
        private readonly ISocialShare _socialShare;

        public ArticleViewModel(
            IBlogRepository blogRepository,
            INavigationService navigationService,
            ISocialShare socialShare
            )
        {
            _blogRepository = blogRepository;
            _navigationService = navigationService;
            _socialShare = socialShare;

            GoToCommentsCommand = new RelayCommand(
                () => _navigationService.Navigate("RichComments", Category + ";;" + Link));

            ShareCommand = new RelayCommand(
                () => _socialShare.ShareLink(Title, new Uri(Link, UriKind.RelativeOrAbsolute)));
            
            ChangePageCommand = new RelayCommand<string>(
                async p =>
                {
                    IsLoading = true;

                    var cts = new CancellationTokenSource();
                    var command = p.Split(new[] {':'}, 2);
                    switch (command[0])
                    {
                        case "reload":
                            var url = command[1];
                            Html = await _blogRepository.GetArticleContentAsync(url, cts.Token);
                            break;
                        case "youtube":
                            var id = command[1];
                            await
                                YouTube.PlayWithPageDeactivationAsync(
                                    id, false, YouTubeQuality.QualityMedium);
                            break;
                    }

                    IsLoading = false;
                }
                );
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _lead;
        public string Lead
        {
            get
            {
                return _lead;
            }
            set
            {
                _lead = value;
                RaisePropertyChanged(() => Lead);
            }
        }

        private string _html;
        public string Html
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

        private string _link;
        public string Link
        {
            get
            {
                return _link;
            }
            set
            {
                _link = value;
                RaisePropertyChanged(() => Link);
            }
        }

        private string _category;
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
                RaisePropertyChanged(() => Category);
            }
        }

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

        public async Task LoadAsync(Article article, CancellationToken cancellationToken)
        {
            IsLoading = true;

            Title = article.Title;
            Link = article.Link;
            
            Html = "";
            if (article.CommentsCount == null)
            {
                Lead = string.Format("{0:00}.{1:00}.{2:0000} | {3}", article.PublishingDate.Day,
                    article.PublishingDate.Month, article.PublishingDate.Year, article.Category);
            }
            else
            {
                Lead = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day,
                    article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount,
                    Resources.AppResources.Lead_Comments);
            }

            var htmlTask = _blogRepository.GetArticleContentAsync(Link, cancellationToken);
            var commentsInfoTask = _blogRepository.GetCommentsInfoAsync(Link, cancellationToken);
            var result = await htmlTask;
            if (!result.Successful)
                _navigationService.Navigate("Error");
            Html = result;
            var commentsInfo = await commentsInfoTask;
            if (commentsInfo.Successful)
                article.CommentsCount = commentsInfo.Value.Count;
            Lead = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
            
            IsLoading = false;
        }

        public async Task LoadInfoAsync(Article article, CancellationToken cancellationToken)
        {
            IsLoading = true;

            Title = article.Title;
            Link = article.Link;

            if (article.CommentsCount == null)
            {
                Lead = string.Format("{0:00}.{1:00}.{2:0000} | {3}", article.PublishingDate.Day,
                    article.PublishingDate.Month, article.PublishingDate.Year, article.Category);
            }
            else
            {
                Lead = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day,
                    article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount,
                    Resources.AppResources.Lead_Comments);
            }

            var commentsCountTask = _blogRepository.GetCommentsInfoAsync(Link, cancellationToken);
            var commentResult = await commentsCountTask;
            if (commentResult.Successful)
                article.CommentsCount = commentResult.Value.Count;
            Lead = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);

            IsLoading = false;
        }

        public ICommand GoToCommentsCommand { get; set; }

        public ICommand ShareCommand { get; set; }

        public ICommand ChangePageCommand { get; set; }

        public ICommand FlipArticleHorizontalCommand { get; set; }
    }
}
