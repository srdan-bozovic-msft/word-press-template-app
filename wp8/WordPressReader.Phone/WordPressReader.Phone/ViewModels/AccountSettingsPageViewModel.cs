using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
using WordPressReader.Phone.Contracts.ViewModels;

namespace WordPressReader.Phone.ViewModels
{
    public class AccountSettingsPageViewModel : ViewModelBase, IAccountSettingsPageViewModel
    {
        private ISettingsRepository _settingsRepository;
        private INavigationService _navigationService;
 
        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        private GuestUserAccount _guestUserAccount;
        private GuestUserAccount GuestUserAccount
        {
            get
            {
                return _guestUserAccount;
            }
            set
            {
                _guestUserAccount = value;
                RaisePropertyChanged(() => UserName);
                RaisePropertyChanged(() => Email); 
                RaisePropertyChanged(() => HasData);
            }
        }

        public string UserName
        {
            get
            {
                return GuestUserAccount.UserName;
            }
            set
            {
                GuestUserAccount.UserName = value;
                _settingsRepository.SetGuestUserAccount(GuestUserAccount);
                RaisePropertyChanged(() => UserName);
                RaisePropertyChanged(() => HasData);
            }
        }

        public string Email
        {
            get
            {
                return GuestUserAccount.Email;
            }
            set
            {
                GuestUserAccount.Email = value;
                _settingsRepository.SetGuestUserAccount(GuestUserAccount);
                RaisePropertyChanged(() => Email);
                RaisePropertyChanged(() => HasData);
            }
        }

        public bool HasData
        {
            get
            {
                return !GuestUserAccount.IsEmpty;
            }
        }

        public ICommand ClearDataCommand { get; set; }

        public AccountSettingsPageViewModel(ISettingsRepository settingsRepository, INavigationService navigationService)
        {
            _settingsRepository = settingsRepository;
            _navigationService = navigationService;
            _guestUserAccount = new GuestUserAccount();
            ClearDataCommand = new RelayCommand(
                () => {
                    UserName = "";
                    Email = "";
                });
        }

        public async Task InitializeAsync(dynamic parameter)
        {
            IsLoading = true;
            GuestUserAccount = _settingsRepository.GetGuestUserAccount();
            IsLoading = false;
        }
    }
}
