using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPressReader.Phone.Contracts.Models
{
    public class GuestUserAccount
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        
        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(UserName) &&
                    string.IsNullOrEmpty(Email);
            }
        }
    }
}
