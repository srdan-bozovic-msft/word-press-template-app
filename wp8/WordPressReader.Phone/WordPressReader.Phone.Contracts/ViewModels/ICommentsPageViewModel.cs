using MSC.Phone.Shared.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WordPressReader.Phone.Contracts.Models;

namespace WordPressReader.Phone.Contracts.ViewModels
{
    public interface ICommentsPageViewModel : IPageViewModel
    {
        ObservableCollection<Comment> Comments { get; }
    }
}
