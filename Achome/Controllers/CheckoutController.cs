using Achome.DbModels;
using Achome.Models;
using Achome.Models.RequestModels;
using Achome.Models.ResponseModels;
using Achome.Service;
using Achome.Service.Implement;
using Achome.Util;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController:ControllerBase
    {
        private readonly ApplicationSettings appSettings;
        private readonly AChomeContext context;
        private readonly ICheckoutService checkoutService;
        private readonly IMapper mapper;
        public CheckoutController(IOptions<ApplicationSettings> appSettings, AChomeContext context, IMapper mapper,
            ICheckoutService checkoutService,ILogger<CheckoutService> logger)
        {
            logger.LogInformation("CheckoutServiceController");
            this.appSettings = appSettings?.Value;
            this.context = context;
            this.checkoutService = checkoutService;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public BaseResponse<List<TransportMethodViewModel>> GetTransportMethodList()
        {
            return this.checkoutService.GetTransportMethodList();
        }

        [HttpGet("[action]")]
        public BaseResponse<Dictionary<string, List<AreaZip>>> GetTaiwanCityList()
        {
            return this.checkoutService.GetTaiwanCityList();
        }

        [HttpGet("[action]")]
        public BaseResponse<List<SevenElevenShopViewModel>> GetSevenElevenShop()
        {
            return this.checkoutService.GetSevenElevenShop();
        }

        [Authorize]
        [HttpPost("[action]")]
        public BaseResponse<bool> CheckingOut([FromBody]CheckOutOrderRawReqestModel checkoutOrder)
        {
            //var Account = "tychang";
            var account = User.Claims.Where(c => c.Type.Equals(ClaimString.AccountName, StringComparison.InvariantCulture)).FirstOrDefault().Value;
            return this.checkoutService.CheckingOut(checkoutOrder, account);
        }
    }
}
