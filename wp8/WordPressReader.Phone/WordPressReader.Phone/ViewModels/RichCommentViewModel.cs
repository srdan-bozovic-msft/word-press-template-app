using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WordPressReader.Phone.Contracts.Models;
using WordPressReader.Phone.Contracts.ViewModels;

namespace WordPressReader.Phone.ViewModels
{
    public class RichCommentViewModel : ViewModelBase, IRichCommentViewModel
    {
        private Comment _comment;
        public RichCommentViewModel(Comment comment)
        {
            if(!comment.AuthorAvatarUrl.StartsWith("http"))
            {
                comment.AuthorAvatarUrl = "http:" + comment.AuthorAvatarUrl;
            }
            _comment = comment;
        }

        public Comment Comment
        {
            get
            {
                return _comment;
            }
        }

        public Thickness Margin 
        {
            get
            {
                return new Thickness(Math.Min(_comment.Level * 25, 100), 0, 0, 0);
            }
        }

        public string Author 
        { 
            get
            {
                return _comment.Author;
            }
        }

        public string AuthorAvatarUrl 
        {
            get
            {
                return _comment.AuthorAvatarUrl;
            }
            
        }

        public string Text
        {
            get
            {
                return _comment.RawContent;
            }
        }

        public string CreatedAt
        {
            get
            {
                var date = _comment.CreatedAt.ToLocalTime();
                var now = DateTime.Now;
                if (date >= now.AddSeconds(-5))
                {
                    return Resources.AppResources.CreatedAt_Just_a_moment_ago;
                }
                if (date >= now.AddSeconds(-60))
                {
                    return (int)((now - date).TotalSeconds) + " " + Resources.AppResources.CreatedAt_seconds_ago;
                }
                if (date >= now.AddSeconds(-120))
                {
                    return Resources.AppResources.CreatedAt_a_minute_ago;
                } 
                if (date >= now.AddMinutes(-60))
                {
                    return (int)((now - date).TotalMinutes) + " " + Resources.AppResources.CreatedAt_minutes_ago;
                }
                if (date >= now.AddHours(-24))
                {
                    return (int)((now - date).TotalHours) + " " + Resources.AppResources.CreatedAt_hours_ago;
                }
                if (date >= now.AddHours(-48))
                {
                    return Resources.AppResources.CreatedAt_yesterday;
                } 
                if (date >= now.AddDays(-7))
                {
                    return (int)((now - date).TotalDays) + " " + Resources.AppResources.CreatedAt_days_ago;
                }
                if (date.Year == now.Year)
                {
                    return date.ToString("dd MMM");
                }
                return date.ToString("dd MMM yyyy");
            }
        }

    }
}
