using System;
using System.Collections.Generic;

namespace Achome.DbModels
{
    public partial class Account
    {
        public string AccountName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime Registertime { get; set; }
        public string PhoneNumber { get; set; }
    }
}
