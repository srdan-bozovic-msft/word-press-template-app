using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WordPressReader.Phone.Contracts.Views;
using MSC.Phone.Shared.Contracts.ViewModels;
using MSC.Phone.Shared.Controls.LinqToVisualTree;
using System.Windows.Input;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace WordPressReader.Phone.Views
{
    public partial class ArticleListPageView : PhoneApplicationPage, IArticlePageView
    {
        public ArticleListPageView()
        {
            InitializeComponent();
        }

        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }
    }
}