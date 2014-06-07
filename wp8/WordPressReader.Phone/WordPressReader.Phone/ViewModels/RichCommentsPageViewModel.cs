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

        public RichCommentsPageViewModel(IBlogRepository blogRepository, INavigationService navigationService)
        {
            _blogRepository = blogRepository;
            _navigationService = navigationService;
            _comments = new ObservableCollection<IRichCommentViewModel>();
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            IsLoading = true;
            HasComments = true;
            var cts = new CancellationTokenSource();
            var parameters = ((string)parameter).Split(new[] { ";;" }, 2, StringSplitOptions.RemoveEmptyEntries);
            _category = parameters[0];
            var articles = await _blogRepository.GetArticlesAsync(_category, false, cts.Token);
            if (!articles.IsError)
            {
                var article = articles.Value.FirstOrDefault(a => a.Link == parameters[1]);
                if (article != null)
                {
                    Title = article.Title;
                    Lead = article.CommentsCount == null ?
                        string.Format("{0:00}.{1:00}.{2:0000} | {3}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category)
                        :
                        string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, article.CommentsCount, Resources.AppResources.Lead_Comments);
                    
                    _comments.Clear();
                    var comments = await _blogRepository.GetCommentsAsync(article, cts.Token);
                    if (!comments.IsError)
                    {
                        //if (comments.Value.Count() > 0)
                        foreach (var comment in comments.Value)
                        {
                            _comments.Add(new RichCommentViewModel(comment));
                        }
                        HasComments = _comments.Count > 0;
                        if (HasComments)
                        {
                            Lead = string.Format("{0:00}.{1:00}.{2:0000} | {3} | {4} {5}", article.PublishingDate.Day, article.PublishingDate.Month, article.PublishingDate.Year, article.Category, _comments.Count, Resources.AppResources.Lead_Comments);
                        }
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
