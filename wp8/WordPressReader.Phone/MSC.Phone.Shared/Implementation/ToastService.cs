using Microsoft.Phone.Shell;
using MSC.Phone.Shared.Contracts.PhoneServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.Services
{
    public class ToastService : IToastService
    {
        public void Show(string title, string content)
        {
            ShellToast toast = new ShellToast();
            toast.Title = title;
            toast.Content = content;
            toast.Show();
        }

        public void Show(string title, string content, Uri uri)
        {
            ShellToast toast = new ShellToast();
            toast.Title = title;
            toast.Content = content;
            toast.NavigationUri = uri;
            toast.Show();
        }
    }
}
