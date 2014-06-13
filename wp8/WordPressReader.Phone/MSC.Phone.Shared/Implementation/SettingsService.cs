using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Implementation
{
    public class SettingsService : ISettingsService
    {
        public void Set(string key, object value)
        {
            IsolatedStorageSettings.ApplicationSettings[key] = value;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public T Get<T>(string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                return (T)IsolatedStorageSettings.ApplicationSettings[key];
            }
            else
            {
                return default(T);
            }
        }

        public bool ContainsKey(string key)
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(key);
        }
    }
}
