using MSC.Phone.Shared.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WordPressReader.Phone.Contracts.Models;

namespace WordPressReader.Phone.Contracts.Repositories
{
    public interface INotificationRepository
    {
        RepositoryResult<bool> ClearNotifications(Article article);
        Task<RepositoryResult<bool>> UpdateNotificationsAsync(CancellationToken cancellationToken);
    }
}
