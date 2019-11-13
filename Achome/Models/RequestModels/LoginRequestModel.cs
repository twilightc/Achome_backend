using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Models.RequestModels
{
    public class LoginRequestModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
