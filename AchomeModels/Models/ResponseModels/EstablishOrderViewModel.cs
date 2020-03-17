using AchomeModels.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Models.ResponseModels
{
    public class EstablishOrderResponse : BaseResponse<List<EstablishOrderViewModel>>
    {
        public EstablishOrderResponse()
        {
        }
        public EstablishOrderResponse(bool success, string msg, List<EstablishOrderViewModel> data) : base(success, msg, data)
        {
        }
    }

    public class EstablishOrderViewModel
    {
        public string OrderGuid { get; set; }
        //public int Status { get; set; }
        public int OriginPrice { get; set; }
        public int AdditionalFee { get; set; }
        //public int DiscountFee { get; set; }
        public int TotalPrice { get; set; }
        public DateTime OrderingTime { get; set; }
        public string TransportName { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhone { get; set; }

        public string SellerAccount { get; set; }

        public string SellerName { get; set; }

        public List<EstablishOrderDetail> OrderDetails { get; set; }
    }

    public class EstablishOrderDetail
    {
        public int Seq { get; set; }
        public string ProdId { get; set; }
        public string ProdName { get; set; }
        public string Spec1 { get; set; }
        public string Spec2 { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
        public int MinorTotal { get; set; }

    }

}
