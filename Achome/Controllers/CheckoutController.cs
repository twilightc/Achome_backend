using Achome.DbModels;
using Achome.Models;
using Achome.Models.ResponseModels;
using Achome.Service;
using Achome.Service.Implement;
using AutoMapper;
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
        public BaseResponse<List<TaiwanCityViewModel>> GetTaiwanCityList()
        {
            return this.checkoutService.GetTaiwanCityList();
        }

        [HttpGet("[action]")]
        public BaseResponse<List<SevenElevenShopViewModel>> GetSevenElevenShop()
        {
            return this.checkoutService.GetSevenElevenShop();
        }
    }
}
