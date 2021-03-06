﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Models.ResponseModels
{
    public class TransportMethodViewModel
    {
        public int TransportId { get; set; }
        public string TransportName { get; set; }
        public int Fee { get; set; }
    }

    //public class TaiwanCityViewModel
    //{
    //    public string City { get; set; }
    //    public string Area { get; set; }
    //    public string Zip { get; set; }
    //}

    public class SevenElevenShopViewModel
    {
        public string Zip { get; set; }
        public string ShopNumber { get; set; }
        public string ShopName { get; set; }
        public string Address { get; set; }

    }

    public class AreaZip
    {
        public AreaZip(string area, string zip)
        {
            Area = area;
            Zip = zip;
        }
        public string Area { get; set; }
        public string Zip { get; set; }
    }
}
