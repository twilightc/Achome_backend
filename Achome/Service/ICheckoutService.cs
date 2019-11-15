using Achome.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Service
{
    public interface ICheckoutService
    {
        BaseResponse<List<TransportMethodViewModel>> GetTransportMethodList();
        BaseResponse<List<TaiwanCityViewModel>> GetTaiwanCityList();
        BaseResponse<List<SevenElevenShopViewModel>> GetSevenElevenShop();
    }
}
