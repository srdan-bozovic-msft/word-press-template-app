using System;
using System.Windows;
using WordPressReader.Phone.Contracts.Models;
namespace WordPressReader.Phone.Contracts.ViewModels
{
    public interface IRichCommentViewModel
    {
        Comment Comment { get; }
        string Author { get; }
        string AuthorAvatarUrl { get; }
        string CreatedAt { get; }
        Thickness Margin { get; }
        string Text { get; }
    }
}
