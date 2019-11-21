using Achome.DbModels;
using Achome.Models;
using Achome.Models.RequestModels;
using Achome.Models.ResponseModels;
using Achome.Service;
using Achome.Util;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationSettings appSettings;
        private readonly AChomeContext context;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IMapper mapper;
        public ShoppingCartController(IOptions<ApplicationSettings> appSettings, IShoppingCartService shoppingCartService, AChomeContext context, IMapper mapper)
        {
            this.appSettings = appSettings?.Value;
            this.context = context;
            this.shoppingCartService = shoppingCartService;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpPost("[action]")]
        public BaseResponse<bool> AddToShoppingCart([FromBody]ShoppingCart shoppingCartModel)
        {
            if (shoppingCartModel == null)
            {
                return new BaseResponse<bool>(false, "model data is null", default);
            }
            var Account = User.Claims.Where(c => c.Type.Equals(ClaimString.AccountName, StringComparison.InvariantCulture)).FirstOrDefault().Value;
            shoppingCartModel.Account = Account;

            return this.shoppingCartService.AddToShoppingCart(shoppingCartModel);
        }

        [Authorize]
        [HttpGet("[action]")]
        public BaseResponse<List<ShoppingCartWrapper>> GetShoppingCart()
        {
            //var Account = "tychang";
            var Account = User.Claims.Where(c => c.Type.Equals(ClaimString.AccountName, StringComparison.InvariantCulture)).FirstOrDefault().Value;
            return this.shoppingCartService.GetShoppingCart(Account);
        }

        [Authorize]
        [HttpPut("[action]")]
        public BaseResponse<bool> RemoveShoppingCartItem(List<PendedShoppingCartItemRequestModel> items)
        {
            //var Account = "tychang";
            var Account = User.Claims.Where(c => c.Type.Equals(ClaimString.AccountName, StringComparison.InvariantCulture)).FirstOrDefault().Value;
            return this.shoppingCartService.RemoveShoppingCartItem(items, Account);
        }
    }
}
