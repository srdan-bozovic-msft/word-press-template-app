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
using WordPressReader.Phone.Contracts.ViewModels;

namespace WordPressReader.Phone.Views
{
    public partial class CategoryPageView : PhoneApplicationPage, ICategoryPageView
    {
        public CategoryPageView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemTray.SetIsVisible(this, true);
            base.OnNavigatedTo(e);
        }

        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }

        private async void ArticleList_ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            if (!ViewModel.IsLoading && ArticleList.ItemsSource != null && ArticleList.ItemsSource.Count >= 2)
            {
                if (e.ItemKind == LongListSelectorItemKind.Item)
                {
                    if ((e.Container.Content).Equals(ArticleList.ItemsSource[ArticleList.ItemsSource.Count - 2]))
                    {
                        await (ViewModel as ICategoryPageViewModel).GetMoreArticlesAsync();
                    }
                }
            }
        }
    }
}