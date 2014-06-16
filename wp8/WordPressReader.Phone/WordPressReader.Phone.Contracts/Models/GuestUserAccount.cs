using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordPressReader.Phone.Contracts.Models
{
    public class GuestUserAccount
    {
        public const string EMAIL_REGEX_PATTERN = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

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

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(UserName) &&
                    !string.IsNullOrEmpty(Email) &&
                    Regex.IsMatch(Email, EMAIL_REGEX_PATTERN);
            }
        }
    }
}
