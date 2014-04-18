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
    public class CommentsPageViewModel : ViewModelBase, ICommentsPageViewModel
    {
        private IBlogRepository _blogRepository;
        private INavigationService _navigationService;

        private readonly ObservableCollection<Comment> _comments;
        public ObservableCollection<Comment> Comments
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

        public CommentsPageViewModel(IBlogRepository blogRepository, INavigationService navigationService)
        {
            _blogRepository = blogRepository;
            _navigationService = navigationService;
            _comments = new ObservableCollection<Comment>();
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            IsLoading = true;
            HasComments = true;
            var cts = new CancellationTokenSource();
            var articles = await _blogRepository.GetArticlesAsync(false, cts.Token);
            if (!articles.IsError)
            {
                var article = articles.Value.FirstOrDefault(a => a.Link == parameter);
                if (article != null)
                {
                    Title = article.Title;
                    Lead = string.Format("{0:00}.{1:00}.{2:0000} | {3}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category);
                    var comments = await _blogRepository.GetCommentsAsync(article.CommentLink, cts.Token);
                    if (!comments.IsError)
                    {
                        //if (comments.Value.Count() > 0)
                        _comments.Clear();
                        foreach (var comment in comments.Value)
                        {
                            comment.Content = Utility.HtmlDecode(comment.Content);
                            _comments.Add(comment);
                        }
                        HasComments = _comments.Count > 0;
                    }
                    else
                    {
                        _navigationService.Navigate("Error");
                    }
                }
            }
            else
            {
                _navigationService.Navigate("Error");
            }
            IsLoading = false;
        }
    }
}
