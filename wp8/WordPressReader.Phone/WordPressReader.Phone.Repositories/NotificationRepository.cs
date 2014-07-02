using Microsoft.Phone.Shell;
using MSC.Phone.Shared.Contracts.Models;
using MSC.Phone.Shared.Contracts.PhoneServices;
using MSC.Phone.Shared.Contracts.Repositories;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WordPressReader.Phone.Contracts.Models;
using WordPressReader.Phone.Contracts.Repositories;
using WordPressReader.Phone.Contracts.Services;

namespace WordPressReader.Phone.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private const string SettingsNotificationsCount = "SettingsNotificationsCount";
        private const string SettingsNotificationsLatestUrl = "SettingsNotificationsLatestUrl";

        private IHttpClientService _httpClientService;
        private IConfigurationService _configurationService;
        private IApplicationSettingsService _applicationSettingsService;
        private ISettingsService _settingsService;
        private ITileService _tileService;
        private IToastService _toastService;

        public NotificationRepository(IHttpClientService httpClientService,
            IConfigurationService configurationService,
            IApplicationSettingsService applicationSettingsService,
            ISettingsService settingsService,
            ITileService tileService,
            IToastService toastService)
        {
            _httpClientService = httpClientService;
            _configurationService = configurationService;
            _applicationSettingsService = applicationSettingsService;
            _settingsService = settingsService;
            _tileService = tileService;
            _toastService = toastService;
        }

        public RepositoryResult<bool> ClearNotifications(Article article)
        {
            try
            {
                _settingsService.Set(SettingsNotificationsCount,0);
                _settingsService.Set(SettingsNotificationsLatestUrl, article.Link);
                _tileService.UpdateTile(new IconicTileData {
                    Count = 0, 
                    IconImage = new Uri("/Assets/Tiles/IconicTileMediumLarge.png", UriKind.Relative),
                    SmallIconImage = new Uri("/Assets/Tiles/IconicTileSmall.png", UriKind.Relative),
                });
                return true;
            }
            catch(Exception xcp)
            {
                return RepositoryResult<bool>.CreateError(xcp);
            }
        }

        public async Task<RepositoryResult<bool>> UpdateNotificationsAsync(CancellationToken cancellationToken)
        {
            try
            {
                if(_settingsService.ContainsKey(SettingsNotificationsCount))
                {
                    var count = _settingsService.Get<int>(SettingsNotificationsCount);
                    var latestUrl = _settingsService.Get<string>(SettingsNotificationsLatestUrl);
                    var feedUrl = _configurationService.GetFeedUrl("<default>");
                    var feed = await _httpClientService.GetXmlAsync<RssFeed>(feedUrl, cancellationToken);
                    var newItems = new List<RssFeed.RssFeedChannel.RssFeedItem>();
                    foreach (var item in feed.Channel.Items)
                    {
		                if(item.Link != latestUrl)
                        {
                            newItems.Add(item);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if(newItems.Count > 0)
                    {
                        _settingsService.Set(SettingsNotificationsLatestUrl, newItems.First().Link);
                        var newCount = count + newItems.Count;
                        _tileService.UpdateTile(new IconicTileData
                        {
                            Count = newCount,
                            IconImage = new Uri("/Assets/Tiles/IconicTileMediumLargeLeft.png", UriKind.Relative),
                            SmallIconImage = new Uri("/Assets/Tiles/IconicTileSmallLeft.png", UriKind.Relative),
                        });
                        _settingsService.Set(SettingsNotificationsCount, newCount);
                        newItems.Reverse();
                        foreach (var item in newItems)
                        {
                            _toastService.Show(item.Creator, item.Title);
                        }
                    }
                }
                return true;
            }
            catch(Exception xcp)
            {
                return RepositoryResult<bool>.CreateError(xcp);
            }
        }
    }
}
