using AchomeModels.DbModels;
using AchomeModels.Models;
using AchomeModels.Models.ResponseModels;
using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace AchomeModels.Service.Implement
{
    public class EstablishedOrderService : IEstablishedOrderService
    {
        private readonly ApplicationSettings appSettings;
        private readonly AChomeContext context;
        private readonly IMapper mapper;
        private readonly ILogger logger;
        public EstablishedOrderService(IOptions<ApplicationSettings> appSettings, AChomeContext context, IMapper mapper, ILogger<EstablishedOrderService> logger)
        {
            logger.LogInformation("EstablishedOrderService");
            this.appSettings = appSettings?.Value;
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }


        public async Task<EstablishOrderResponse> GetEstablishOrderList(string account)
        {
            var establishOrderResponse = new EstablishOrderResponse();
            try
            {
                using (var con = new SqlConnection(appSettings.IdentityConnection))
                {
                    //await con.OpenAsync().ConfigureAwait(false);
                    establishOrderResponse.Data = con.Query<EstablishOrderViewModel>(@"
                  select OrderGuid,OriginPrice,AdditionalFee,TotalPrice,OrderingTime,TransportName,ReceiverName,ReceiverAddress,ReceiverPhone,SellerAccount,SellerName from [Order] a
                  left join TransportMethod b on a.TransportType= b.TransportId
                  outer apply (
	                select top 1 Account.AccountName as SellerAccount,Account.UserName as SellerName from OrderDetail 
	                left join Merchandise on OrderDetail.ProdId=Merchandise.MerchandiseId 
	                left join Account on Merchandise.OwnerAccount=Account.AccountName
	                where OrderDetail.OrderGuid = a.OrderGuid) c
                  where a.OrderAccount=@account", new { account }).ToList();
                    establishOrderResponse.Data.ForEach(data =>
                    {
                        data.OrderDetails = con.Query<EstablishOrderDetail>(@"  select Seq,ProdId,b.MerchandiseTitle as ProdName,Spec1,Spec2,b.Price,Qty from [OrderDetail] a
                          left join Merchandise b on a.ProdId=b.MerchandiseId
                          left join MerchandiseSpec c on a.ProdId=c.MerchandiseId and a.SpecId=c.SpecId
                          where a.OrderGuid=@OrderGuid", new { data.OrderGuid }).ToList();
                        data.OrderDetails.ForEach(detail =>
                        {
                            detail.MinorTotal = detail.Price * detail.Qty;
                        });
                    });
                    establishOrderResponse.Success = true;
                    establishOrderResponse.Msg = "Get EstablishOrderList:";
                    return establishOrderResponse;
                }
            }
            catch (Exception ex)
            {
                establishOrderResponse.Msg = ex.Message;
                return establishOrderResponse;
            }
        }
    }
}
