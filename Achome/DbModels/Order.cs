using System;
using System.Collections.Generic;

namespace Achome.DbModels
{
    public partial class Order
    {
        public string OrderGuid { get; set; }
        public int Status { get; set; }
        public int OriginPrice { get; set; }
        public int AdditionalFee { get; set; }
        public int DiscountFee { get; set; }
        public int TotalPrice { get; set; }
        public DateTime OrderingTime { get; set; }
        public string OrderAccount { get; set; }
        public int TransportType { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhone { get; set; }
    }
}
