using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Models.ResponseModels
{
    public class TransportMethodViewModel
    {
        public int TransportId { get; set; }
        public string TransportName { get; set; }
        public int Fee { get; set; }
    }

    public class TaiwanCityViewModel
    {
        public string City { get; set; }
        public string Area { get; set; }
        public string Zip { get; set; }
    }

    public class SevenElevenShopViewModel
    {
        public string Zip { get; set; }
        public string ShopNumber { get; set; }
        public string ShopName { get; set; }
        public string Address { get; set; }

    }
}
