using System;
using System.Collections.Generic;

namespace AchomeModels.DbModels
{
    public partial class OrderDetail
    {
        public string OrderGuid { get; set; }
        public int Seq { get; set; }
        public string ProdId { get; set; }
        public int SpecId { get; set; }
        public int Qty { get; set; }
        public int TotalPrice { get; set; }
    }
}
