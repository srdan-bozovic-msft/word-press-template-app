using Microsoft.Phone.Tasks;
using MSC.Phone.Shared.Contracts.PhoneServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Implementation
{
    public class SocialShare : ISocialShare
    {
        public void ShareStatus(string status)
        {
            var task = new ShareStatusTask();
            task.Status = status;
            task.Show();
        }

        public void ShareLink(string title, string message, Uri linkUri)
        {
            var task = new ShareLinkTask();
            task.LinkUri = linkUri;
            task.Title = title;
            task.Message = message;
            task.Show();
        }

        public void ShareLink(string title, Uri linkUri)
        {
            var task = new ShareLinkTask();
            task.LinkUri = linkUri;
            task.Title = title;
            task.Show();
        }
    }
}
