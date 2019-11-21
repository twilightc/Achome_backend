using Achome.DbModels;
using Achome.Models.RequestModels;
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
        BaseResponse<Dictionary<string, List<AreaZip>>> GetTaiwanCityList();
        BaseResponse<List<SevenElevenShopViewModel>> GetSevenElevenShop();

        BaseResponse<bool> CheckingOut(CheckOutOrderRawReqestModel CheckoutOrder,string account);
    }
}
