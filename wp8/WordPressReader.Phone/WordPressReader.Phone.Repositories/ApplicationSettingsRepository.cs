using MSC.Phone.Shared.Contracts.Repositories;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPressReader.Phone.Contracts.Models;
using WordPressReader.Phone.Contracts.Repositories;
using WordPressReader.Phone.Contracts.Services;

namespace WordPressReader.Phone.Repositories
{
    public class ApplicationSettingsRepository : IApplicationSettingsRepository
    {
        private IApplicationSettingsService _applicationSettingsService;

        public ApplicationSettingsRepository(IApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;
        }

        public RepositoryResult<GuestUserAccount> GetGuestUserAccount()
        {
            return _applicationSettingsService.GetGuestUserAccount();
        }

        public void SetGuestUserAccount(GuestUserAccount guestUser)
        {
            _applicationSettingsService.SetGuestUserAccount(guestUser);
        }

        public void ClearAccountData()
        {
            _applicationSettingsService.ClearAccountData();
        }
    }
}
