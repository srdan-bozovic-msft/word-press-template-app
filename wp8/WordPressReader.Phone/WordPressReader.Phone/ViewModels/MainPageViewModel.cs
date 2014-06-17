using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using MSC.Phone.Shared.Contracts.Models;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WordPressReader.Phone.Contracts.Models;
using WordPressReader.Phone.Contracts.Repositories;
using WordPressReader.Phone.Contracts.Services;
using WordPressReader.Phone.Contracts.ViewModels;

namespace WordPressReader.Phone.ViewModels
{
    public class MainPageViewModel : CategoryPageViewModel, IMainPageViewModel
    {
        private readonly ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
        }

        public MainPageViewModel(
            IBlogRepository blogRepository,
            INotificationRepository notificationRepository, 
            INavigationService navigationService)
            : base(blogRepository, notificationRepository, navigationService)
        {
            _categories = new ObservableCollection<Category>();
            SelectCategoryCommand = new RelayCommand<Category>(
                category =>
                    _navigationService.Navigate("Category", category.Title + ";;" + category.Tag)
                );
            GoToSettingsCommand = new RelayCommand(
                () =>
                    _navigationService.Navigate("AccountSettings", "")
                );
        }

        public override async Task InitializeAsync(dynamic parameter)
        {
            ClientAnalyticsChannel.Default.LogPageView("Phone/Category/<default>");
            await InitializeInternalAsync(";;<default>");
        }

        protected override async Task InitializeInternalAsync(string category)
        {
            await ReloadCategoriesAsync();
            var parts = category.Split(new[] { ";;" }, StringSplitOptions.None);
            PageTitle = parts[0];
            _category = parts[1];
            await ReloadArticlesAsync(false);
        }

        private async Task ReloadCategoriesAsync()
        {
            IsLoading = true;
            var cts = new CancellationTokenSource();
            _categories.Clear();
            _categories.Add(new Category
            {
                BackColor = "#8cbd34",
                Tag="nokia",
                Title="Nokia",
                ForeColor="#FFFFFF",
                Wide= false
            });
            _categories.Add(new Category
            {
                BackColor = "#bd01f6",
                Tag = "windows-phone-2",
                Title = "Windows Phone",
                ForeColor = "#FFFFFF",
                Wide = false
            });
            _categories.Add(new Category
            {
                BackColor = "#3191c7",
                Tag = "aplikacije",
                Title = "Aplikacije",
                ForeColor = "#FFFFFF",
                Wide = false
            }); _categories.Add(new Category
            {
                BackColor = "#ff6600",
                Tag = "recenzije",
                Title = "Recenzije",
                ForeColor = "#FFFFFF",
                Wide = false
            });
            IsLoading = false;
        }

        public ICommand SelectCategoryCommand { get; set; }

        public ICommand GoToSettingsCommand { get; set; }
    }
}
