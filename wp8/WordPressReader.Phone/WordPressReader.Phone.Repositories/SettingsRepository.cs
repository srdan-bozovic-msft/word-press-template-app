using MSC.Phone.Shared.Contracts.Repositories;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPressReader.Phone.Contracts.Models;
using WordPressReader.Phone.Contracts.Repositories;

namespace WordPressReader.Phone.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private const string SettingsAccountUserName = "SettingsAccountUserName";
        private const string SettingsAccountEmail = "SettingsAccountEmail";

        private ISettingsService _settingsService;

        public SettingsRepository(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public RepositoryResult<GuestUserAccount> GetGuestUserAccount()
        {
            var userName = _settingsService.Get<string>(SettingsAccountUserName);
            var email = _settingsService.Get<string>(SettingsAccountEmail);
            return new GuestUserAccount { UserName = userName, Email = email };
        }

        public void SetGuestUserAccount(GuestUserAccount guestUser)
        {
            _settingsService.Set(SettingsAccountUserName, guestUser.UserName);
            _settingsService.Set(SettingsAccountEmail, guestUser.Email);
        }

        public void ClearAccountData()
        {
            SetGuestUserAccount(new GuestUserAccount());
        }
    }
}
