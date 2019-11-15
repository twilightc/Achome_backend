using Achome.DbModels;
using Achome.Models;
using Achome.Models.ResponseModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Service.Implement
{
    public class CheckoutService:ICheckoutService
    {
        private readonly ApplicationSettings appSettings;
        private readonly AChomeContext context;
        private readonly IMapper mapper;
        private readonly ILogger logger;
        public CheckoutService(IOptions<ApplicationSettings> appSettings, AChomeContext context, IMapper mapper, ILogger<CheckoutService> logger)
        {
            logger.LogInformation("CheckoutService");
            this.appSettings = appSettings?.Value;
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public BaseResponse<List<TransportMethodViewModel>> GetTransportMethodList()
        {
            try
            {
                var Result = context.TransportMethod.Select(data => mapper.Map<TransportMethod, TransportMethodViewModel>(data)).ToList();

                return new BaseResponse<List<TransportMethodViewModel>>(true, "TransportMethodList", Result);
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
                return new BaseResponse<List<TransportMethodViewModel>>(false, "Cannot get TransportMethodList", null);
            }
        }

        public BaseResponse<List<TaiwanCityViewModel>> GetTaiwanCityList()
        {
            try
            {
                var Result = context.TaiwanCity.Select(data => mapper.Map<TaiwanCity, TaiwanCityViewModel>(data)).ToList();

                return new BaseResponse<List<TaiwanCityViewModel>>(true, "TaiwanCityList", Result);
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
                return new BaseResponse<List<TaiwanCityViewModel>>(false, "Cannot get TaiwanCityList", null);
            }
        }

        public BaseResponse<List<SevenElevenShopViewModel>> GetSevenElevenShop()
        {
            try
            {
                var Result = context.SevenElevenShop.Select(data => mapper.Map<SevenElevenShop, SevenElevenShopViewModel>(data)).ToList();

                return new BaseResponse<List<SevenElevenShopViewModel>>(true, "SevenElevenShopViewModel", Result);
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
                return new BaseResponse<List<SevenElevenShopViewModel>>(false, "Cannot get SevenElevenShopList", null);
            }
        }

    }
}
