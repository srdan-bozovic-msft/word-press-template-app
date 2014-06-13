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

namespace WordPressReader.Phone.Views
{
    public partial class AccountSettingsPageView : PhoneApplicationPage, IAccountSettingsPageView
    {
        public AccountSettingsPageView()
        {
            InitializeComponent();
        }

        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }
    }
}