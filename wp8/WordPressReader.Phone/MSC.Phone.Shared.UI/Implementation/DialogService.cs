using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MSC.Phone.Shared
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }

        public bool ShowConsent(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.OKCancel) == MessageBoxResult.OK;
        }
    }
}
