using System;
using System.Collections.Generic;

namespace Achome.DbModels
{
    public partial class MerchandiseSpec
    {
        public string MerchandiseId { get; set; }
        public int SpecId { get; set; }
        public int Price { get; set; }
        public int SoldQty { get; set; }
        public int RemainingQty { get; set; }
        public string Spec1 { get; set; }
        public string Spec2 { get; set; }
        public bool Enable { get; set; }
    }
}
