using System;
using System.Windows;
namespace WordPressReader.Phone.Contracts.ViewModels
{
    public interface IRichCommentViewModel
    {
        string Author { get; }
        string AuthorAvatarUrl { get; }
        string CreatedAt { get; }
        Thickness Margin { get; }
        string Text { get; }
    }
}
