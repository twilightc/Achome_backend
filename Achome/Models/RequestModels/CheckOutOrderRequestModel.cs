using Achome.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Models.RequestModels
{
    public class CheckOutOrderRawReqestModel
    {
        public List<PendedShoppingCartItemRequestModel> Merchandises { get; set; }
        public string Recipient { get; set; }
        public int TransportId { get; set; }
        public SevenElevenShop ReveiceShop { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhone { get; set; }
    }


}
