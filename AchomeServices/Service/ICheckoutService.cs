using AchomeModels.DbModels;
using AchomeModels.Models.RequestModels;
using AchomeModels.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Service
{
    public interface ICheckoutService
    {
        BaseResponse<List<TransportMethodViewModel>> GetTransportMethodList();
        BaseResponse<Dictionary<string, List<AreaZip>>> GetTaiwanCityList();
        BaseResponse<List<SevenElevenShopViewModel>> GetSevenElevenShop();

        BaseResponse<bool> CheckingOut(CheckOutOrderRawReqestModel CheckoutOrder,string account);
    }
}
