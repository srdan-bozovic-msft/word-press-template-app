using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.Services
{
    public interface ISettingsService
    {
        void Set(string key, object value);
        T Get<T>(string key);
        bool ContainsKey(string key);
    }
}
