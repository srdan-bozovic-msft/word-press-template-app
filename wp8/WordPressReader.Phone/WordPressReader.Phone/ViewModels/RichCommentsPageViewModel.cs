using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
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
using WordPressReader.Phone.Resources;

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

        public string ReplyToText
        {
            get
            {
                return HasReplyTo ? string.Format(AppResources.Comments_ReplyToText, ReplyTo.Author) : "";
            }
        }

        public bool HasReplyTo
        {
            get
            {
                return ReplyTo != null;
            }
        }

        public bool HasCredentials
        {
            get
            {
                return _blogRepository.IsGuestAccountValid();
            }
        }

        private IRichCommentViewModel _replyTo;
        public IRichCommentViewModel ReplyTo
        {
            get
            {
                return _replyTo;
            }
            set
            {
                if (_replyTo != value)
                {
                    _replyTo = value;
                    RaisePropertyChanged(() => HasReplyTo);
                    RaisePropertyChanged(() => ReplyToText);
                }
            }
        }

        private Article Article { get; set; }
        public ICommand SendCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
        public ICommand ReplyToCommand { get; set; }
        public ICommand GoToAccountSettingsCommand { get; set; }
        public RichCommentsPageViewModel(IBlogRepository blogRepository, INavigationService navigationService, IDialogService dialogService)
        {
            _blogRepository = blogRepository;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _comments = new ObservableCollection<IRichCommentViewModel>();
            SendCommand = new RelayCommand(async () =>
            {
                await SendMessageAsync();
            }); 
            ReloadCommand = new RelayCommand(async () =>
            {
                var cts = new CancellationTokenSource();
                IsLoading = true;
                await ReloadCommentsAsync(cts);
                IsLoading = false;
            });
            ReplyToCommand = new RelayCommand<IRichCommentViewModel>(commentViewModel => {
                ReplyTo = commentViewModel;        
            });
            GoToAccountSettingsCommand = new RelayCommand(() =>
            {
                if (_dialogService.ShowConsent(AppResources.Account_NoCredentials_Message, AppResources.Account_NoCredentials_Title))
                {
                    _navigationService.Navigate("AccountSettings");
                }
            });
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            ClientAnalyticsChannel.Default.LogPageView("Phone/Comments");
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

        public void VerifyCredentials()
        {
            RaisePropertyChanged(() => HasCredentials);
        }

        private async Task ReloadCommentsAsync(CancellationTokenSource cts)
        {
            using (TimedAnalyticsEvent token = ClientAnalyticsChannel.Default.StartTimedEvent("Phone/Comments/Reload"))
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
                    token.Cancel();
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
        }

        public async Task SendMessageAsync()
        {            
            ClientAnalyticsChannel.Default.LogEvent("Phone/Comments/Create");

            var cts = new CancellationTokenSource();

            var parent = ReplyTo != null ? ReplyTo.Comment.Id : null;

            //var comment = new RichCommentViewModel(
            //    new Comment { 
            //        Author = "Srki",
            //        AuthorAvatarUrl = "//a.disquscdn.com/1402432716/images/noavatar32.png",
            //        Content = "test", 
            //        CreatedAt = DateTime.Now });
            //InsertComment(comment, parent);
            using (TimedAnalyticsEvent token = ClientAnalyticsChannel.Default.StartTimedEvent("Phone/Comments/Create"))
            {
                var result = await _blogRepository.CreateCommentAsync(Article, Message, parent, cts.Token);
                if (result.Successful)
                {
                    InsertComment(new RichCommentViewModel(result.Value), parent);
                }
                else
                {
                    token.Cancel();
                    _dialogService.ShowMessage(result.ErrorMessage);
                }
            }
        }

        private void InsertComment(RichCommentViewModel comment, string parent)
        {
            if (parent == null)
            {
                _comments.Insert(0, comment);
            }
            else
            {
                for (int i = 0; i < _comments.Count; i++)
                {
                    if (_comments[i].Comment.Id == parent)
                    {
                        comment.Comment.Level = _comments[i].Comment.Level + 1;
                        _comments.Insert(i + 1, comment);
                        break;
                    }
                }
            }
            OnCommentInserted();
        }

        private void OnCommentInserted()
        {
            Message = "";
            ReplyTo = null;
            HasComments = true;
            Article.CommentsCount++;
            Lead = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", Article.PublishingDate.Day, Article.PublishingDate.Month, Article.PublishingDate.Year, Article.Category, Article.CommentsCount, Resources.AppResources.Lead_Comments);
        }


    }
}
