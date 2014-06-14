using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message, string caption = "");
        bool ShowConsent(string message, string caption = "");
    }
}
