using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AchomeModels.DbModels;
using AchomeModels.Models;

namespace AchomeModels.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly AChomeContext context;

        public UserService(AChomeContext context)
        {
            this.context = context;
        }

        public LoginStatus Login(string account, string password)
        {
            password = Util.Util.PasswordEncoding(password);
            var LoginUserInfo = context.Account.FirstOrDefault(u => u.AccountName.Equals(account,StringComparison.InvariantCulture) && u.Password.Equals(password,StringComparison.InvariantCulture));
            if (LoginUserInfo == null)
            {
                return new LoginStatus(false, null, null);
            }
            else
            {
                return new LoginStatus(true, "yee", LoginUserInfo);
            }

        }
        public bool Register(Account user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                Console.WriteLine(user);
                user.Registertime = DateTime.Now;
                user.Password = Util.Util.PasswordEncoding(user.Password);
                context.Account.Add(user);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
