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
using WordPressReader.Phone.Contracts.ViewModels;

namespace WordPressReader.Phone.ViewModels
{
    public class RichCommentsPageViewModel : ViewModelBase, IRichCommentsPageViewModel
    {
        private IBlogRepository _blogRepository;
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private string _category;

        private readonly ObservableCollection<IRichCommentViewModel> _comments;
        public ObservableCollection<IRichCommentViewModel> Comments
        {
            get
            {
                return _comments;
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

        private bool _hasComments;
        public bool HasComments
        {
            get
            {
                return _hasComments;
            }
            set
            {
                _hasComments = value;
                RaisePropertyChanged(() => HasComments);
            }
        }

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        private string _replyToText;
        public string ReplyToText
        {
            get
            {
                return _replyToText;
            }
            set
            {
                _replyToText = value;
                RaisePropertyChanged(() => ReplyToText);
            }
        }

        private bool _hasReplyTo;
        public bool HasReplyTo
        {
            get
            {
                return _hasReplyTo;
            }
            set
            {
                _hasReplyTo = value;
                RaisePropertyChanged(() => HasReplyTo);
            }
        }

        private Article Article { get; set; }

        public ICommand SendCommand { get; set; }

        public ICommand ReloadCommand { get; set; }

        public RichCommentsPageViewModel(IBlogRepository blogRepository, INavigationService navigationService, IDialogService dialogService)
        {
            _blogRepository = blogRepository;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _comments = new ObservableCollection<IRichCommentViewModel>();
            SendCommand = new RelayCommand(SendMessage);
            ReloadCommand = new RelayCommand(async () => {
                var cts = new CancellationTokenSource();
                await ReloadCommentsAsync(cts);
            });
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            IsLoading = true;
            HasComments = true;
            var cts = new CancellationTokenSource();
            var parameters = ((string)parameter).Split(new[] { ";;" }, 2, StringSplitOptions.RemoveEmptyEntries);
            _category = parameters[0];
            var articles = await _blogRepository.GetArticlesAsync(_category, false, cts.Token);
            if (articles.Successful)
            {
                Article = articles.Value.FirstOrDefault(a => a.Link == parameters[1]);
                if (Article != null)
                {
                    Title = Article.Title;
                    Lead = Article.CommentsCount == null ?
                        string.Format("{0:00}.{1:00}.{2:0000} | {3}", Article.PublishingDate.Day, Article.PublishingDate.Month, Article.PublishingDate.Year, Article.Category)
                        :
                        string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", Article.PublishingDate.Day, Article.PublishingDate.Month, Article.PublishingDate.Year, Article.Category, Article.CommentsCount, Resources.AppResources.Lead_Comments);

                    await ReloadCommentsAsync(cts);
                }
            }
            else
            {
                _navigationService.Navigate("Error");
            }
            IsLoading = false;
        }

        private async Task ReloadCommentsAsync(CancellationTokenSource cts)
        {
            _comments.Clear();
            var commentsTask = _blogRepository.GetCommentsAsync(Article, cts.Token);
            var commentsInfoTask = string.IsNullOrEmpty(Article.Id) ? _blogRepository.GetCommentsInfoAsync(Article.Link, cts.Token) : null;
            var comments = await commentsTask;

            if (comments.Successful)
            {
                //if (comments.Value.Count() > 0)
                foreach (var comment in comments.Value)
                {
                    _comments.Add(new RichCommentViewModel(comment));
                }
                HasComments = _comments.Count > 0;
                if (HasComments)
                {
                    Lead = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", Article.PublishingDate.Day, Article.PublishingDate.Month, Article.PublishingDate.Year, Article.Category, _comments.Count, Resources.AppResources.Lead_Comments);
                }
            }
            else
            {
                _navigationService.Navigate("Error");
            }

            if (commentsInfoTask != null)
            {
                var commentsInfo = await commentsInfoTask;
                if (commentsInfo.Successful)
                {
                    Article.Id = commentsInfo.Value.Id;
                }
            }
        }

        private async void SendMessage()
        {
            var cts = new CancellationTokenSource();
            var result = await _blogRepository.CreateCommentAsync(Article, Message, null, cts.Token);
            if(result.Successful)
            {
                _comments.Add(new RichCommentViewModel(result.Value));
            }
            else
            {
                _dialogService.ShowMessage(result.ErrorMessage);
            }
        }


    }
}
