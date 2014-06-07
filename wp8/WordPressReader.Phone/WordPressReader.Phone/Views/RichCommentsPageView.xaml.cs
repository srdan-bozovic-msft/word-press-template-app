using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MSC.Phone.Shared.Contracts.ViewModels;
using WordPressReader.Phone.Contracts.Views;

namespace WordPressReader.Phone.Views
{
    public partial class RichCommentsPageView : PhoneApplicationPage, ICommentsPageView
    {
        public RichCommentsPageView()
        {
            InitializeComponent();
        }

        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }
    }
}