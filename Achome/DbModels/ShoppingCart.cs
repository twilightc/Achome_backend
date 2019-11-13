using System;
using System.Collections.Generic;

namespace Achome.DbModels
{
    public partial class ShoppingCart
    {
        public string Account { get; set; }
        public string ProdId { get; set; }
        public int SpecId { get; set; }
        public int PurchaseQty { get; set; }
        public DateTime AddTime { get; set; }
    }
}
