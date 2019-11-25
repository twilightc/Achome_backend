using Achome.DbModels;
using Achome.Models;
using Achome.Models.RequestModels;
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
    public class CheckoutService : ICheckoutService
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

        public BaseResponse<Dictionary<string, List<AreaZip>>> GetTaiwanCityList()
        {
            try
            {

                var dict = new Dictionary<string, List<AreaZip>>();
                foreach (var city in context.TaiwanCity)
                {
                    if (!dict.ContainsKey(city.City))
                    {
                        dict.Add(city.City, new List<AreaZip>());
                    }
                    dict[city.City].Add(new AreaZip(city.Area, city.Zip));
                }
                return new BaseResponse<Dictionary<string, List<AreaZip>>>(true, "TaiwanCityList", dict);
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
                return new BaseResponse<Dictionary<string, List<AreaZip>>>(false, "Cannot get TaiwanCityList", null);
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

        public BaseResponse<bool> CheckingOut(CheckOutOrderRawReqestModel checkoutOrder,string account)
        {
            try
            {
                if (checkoutOrder == null)
                {
                    throw new ArgumentNullException(nameof(checkoutOrder));
                }
                List<ShoppingCart> cartInfo = new List<ShoppingCart>();
                checkoutOrder.Merchandises.ForEach(info =>
                {
                    var temp = this.context.ShoppingCart.Where(data => data.Account == account && data.ProdId == info.ProdId && data.SpecId == info.SpecId).FirstOrDefault();
                    if (temp != null)
                    {
                        cartInfo.Add(temp);
                    }
                });

                //adding new order by merchandise amounts
                string guid = Guid.NewGuid().ToString();
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                int op = 0;
                cartInfo.Select((info, i) =>
                {
                    var tempMerchandise = this.context.Merchandise.Where(data => data.MerchandiseId.Equals(info.ProdId, StringComparison.InvariantCulture)).FirstOrDefault();
                    op += tempMerchandise.Price * info.PurchaseQty;
                    orderDetails.Add(new OrderDetail()
                    {
                        OrderGuid = guid,
                        Seq = i + 1,
                        ProdId = info.ProdId,
                        SpecId = info.SpecId,
                        Qty = info.PurchaseQty,
                        TotalPrice = tempMerchandise.Price * info.PurchaseQty
                    });
                    return 0;
                }).ToList();
                this.context.OrderDetail.AddRange(orderDetails);
                
                var tempFee = this.context.TransportMethod.Where(data => data.TransportId == checkoutOrder.TransportId).FirstOrDefault();
                Order order = new Order()
                {
                    OrderGuid = guid,
                    Status = 1,
                    OriginPrice = op,
                    AdditionalFee = tempFee.Fee,
                    DiscountFee = 0,
                    TotalPrice = op + tempFee.Fee,
                    OrderingTime = DateTime.Now,
                    OrderAccount = cartInfo.First().Account,
                    TransportType = tempFee.TransportId,
                    ReceiverName = checkoutOrder.Recipient,
                    ReceiverAddress = checkoutOrder.ReceiverAddress,
                    ReceiverPhone = checkoutOrder.ReceiverPhone,
                };
                this.context.Order.Add(order);
                this.context.RemoveRange(cartInfo);
                this.context.SaveChanges();
                return new BaseResponse<bool>(true, "Order has been accepted", true);
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
                return new BaseResponse<bool>(false, "Checkout data pattern failed", default);
            }
        }
    }
}
