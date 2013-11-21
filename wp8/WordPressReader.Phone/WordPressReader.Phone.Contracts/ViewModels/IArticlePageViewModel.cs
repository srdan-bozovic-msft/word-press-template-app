using MSC.Phone.Shared.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPressReader.Phone.Contracts.ViewModels
{
    public interface IArticlePageViewModel : IPageViewModel
    {
        string PageTitle { get; }
        string Html { get; }
    }
}
