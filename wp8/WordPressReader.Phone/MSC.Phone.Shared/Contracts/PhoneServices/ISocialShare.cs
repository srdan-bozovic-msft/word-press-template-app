using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.PhoneServices
{
    public interface ISocialShare
    {
        void ShareStatus(string status);
        void ShareLink(string title, string message, Uri linkUri);
        void ShareLink(string title, Uri linkUri);
    }
}
