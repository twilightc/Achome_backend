using AchomeModels.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Models
{
    public class LoginStatus
    {
        public LoginStatus(bool isLogin, string key, Account user)
        {
            IsLogin = isLogin;
            Key = key;
            User = user;
        }

        public bool IsLogin { get; set; }
        public string Key { get; set; }
        public Account User { get; set; }
    }
}
