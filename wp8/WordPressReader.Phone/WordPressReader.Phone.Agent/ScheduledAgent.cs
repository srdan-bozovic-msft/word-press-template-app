using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using MSC.Phone.Shared;
using System.Threading;
using WordPressReader.Phone.Repositories;
using WordPressReader.Phone.Services;
using MSC.Phone.Shared.Implementation;
using MSC.Phone.Shared.Contracts.Services;

namespace WordPressReader.Phone.Agent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected async override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background
            var httpClientService = new HttpClientService();
            var configurationService = new ConfigurationService();
            var settingsService = new SettingsService();
            var applicationSettingsService = new ApplicationSettingsService(settingsService);
            var tileService = new TileService();
            var toastService = new ToastService();

            var repository = new NotificationRepository(
                httpClientService,
                configurationService,
                applicationSettingsService,
                settingsService,
                tileService,
                toastService);

            var cts = new CancellationTokenSource();

            await repository.UpdateNotificationsAsync(cts.Token);

            NotifyComplete();
        }
    }
}