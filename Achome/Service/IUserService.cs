using Achome.DbModels;
using Achome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Service
{
    public interface IUserService
    {
        LoginStatus Login(string account, string password);
        bool Register(Account user);
    }
}
