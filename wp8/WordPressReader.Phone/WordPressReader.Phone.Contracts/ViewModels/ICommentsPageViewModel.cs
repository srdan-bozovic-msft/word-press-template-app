using MSC.Phone.Shared.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WordPressReader.Phone.Contracts.ViewModels
{
    public interface ICommentsPageViewModel : IPageViewModel
    {
        string PageTitle { get; }
        string Url { get; }
    }
}
