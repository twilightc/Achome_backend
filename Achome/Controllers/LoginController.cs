using Achome.DbModels;
using Achome.Models;
using Achome.Models.RequestModels;
using Achome.Models.ResponseModels;
using Achome.Service;
using Achome.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Achome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService loginService;
        private readonly ApplicationSettings appSettings;
        public AuthController(IUserService loginService, IOptions<ApplicationSettings> appSettings)
        {
            this.loginService = loginService;
            this.appSettings = appSettings?.Value;
        }


        [HttpPost("[action]")]
        public BaseResponse<string> Login([FromBody]LoginRequestModel loginRequestModel)
        {
            var loginStatus = loginService.Login(loginRequestModel?.Account, loginRequestModel.Password);
            if (loginStatus.IsLogin)
            {
                //produce key by using JWT
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT_Secret));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimString.AccountName,loginStatus.User.AccountName),
                        new Claim(ClaimString.UserName,loginStatus.User.UserName),
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return new BaseResponse<string>(true, "", token);
            }

            return new BaseResponse<string>(false, "帳號或密碼不符", null);
        }

        [HttpPost("[action]")]
        public BaseResponse<string> Register([FromBody]Account user)
        {
            bool registerResult = loginService.Register(user);
            if (registerResult)
            {
                return new BaseResponse<string>(registerResult, "", "Register status:true");
            }
            else
            {
                return new BaseResponse<string>(registerResult, "", "Register status:false");
            }

        }


        [Authorize]
        [HttpGet("[action]")]
        public static bool TestAuth()
        {
            return true;
        }
    }
}
