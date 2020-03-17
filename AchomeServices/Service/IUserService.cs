using AchomeModels.DbModels;
using AchomeModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Service
{
    public interface IUserService
    {
        LoginStatus Login(string account, string password);
        bool Register(Account user);
    }
}
