/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:WordPressReader.Phone"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MSC.Phone.Shared;
using MSC.Phone.Shared.Contracts.DI;
using MSC.Phone.Shared.Contracts.PhoneServices;
using MSC.Phone.Shared.Contracts.Services;
using MSC.Phone.Shared.DI;
using WordPressReader.Phone.Contracts.Repositories;
using WordPressReader.Phone.Contracts.Services;
using WordPressReader.Phone.Contracts.ViewModels;
using WordPressReader.Phone.Repositories;
using WordPressReader.Phone.Services;
using WordPressReader.Phone.ViewModels;

namespace WordPressReader.Phone.Common
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public static IInstanceFactory InstanceFactory
        {
            get
            {
                return SimpleIocInstanceFactory.Default;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            var ioc = InstanceFactory;

            ioc.RegisterType<IHttpClientService, HttpClientService>();
            ioc.RegisterType<ICacheService, PhoneStorageCacheService>();
            ioc.RegisterType<INavigationService, NavigationService>();
            ioc.RegisterType<IConfigurationService, ConfigurationService>();
            ioc.RegisterType<IBlogRepository, BlogRepository>();
            ioc.RegisterType<IMainPageViewModel, MainPageViewModel>();
            ioc.RegisterType<IArticlePageViewModel, ArticlePageViewModel>();
            ioc.RegisterType<ICommentsPageViewModel, CommentsPageViewModel>();
        }

        public IMainPageViewModel MainPageViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<IMainPageViewModel>();
            }
        }
        public IArticlePageViewModel ArticlePageViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<IArticlePageViewModel>();
            }
        }

        public ICommentsPageViewModel CommentsPageViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<ICommentsPageViewModel>();
            }
        }
    }
}