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

        public Thickness Margin 
        {
            get
            {
                return new Thickness(0);
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
                var date = _comment.CreatedAt;
                var now = DateTime.Now;
                if (date >= now.AddSeconds(-5))
                {
                    return "Just a moment ago";
                }
                if (date >= now.AddSeconds(-60))
                {
                    return (int)((now - date).TotalSeconds) + " seconds ago";
                }
                if (date >= now.AddMinutes(-60))
                {
                    return (int)((now - date).TotalMinutes) + " minutes ago";
                }
                if (date >= now.AddHours(-24))
                {
                    return (int)((now - date).TotalHours) + " hours ago";
                }
                if (date >= now.AddDays(-7))
                {
                    return (int)((now - date).TotalDays) + " days ago";
                }
                if (date.Year == now.Year)
                {
                    return date.ToString("dd MMM");
                }
                return date.ToString("dd MMM YYYY");
            }
        }

    }
}
