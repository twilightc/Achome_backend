using System;
using System.Collections.Generic;

namespace Achome.DbModels
{
    public partial class Merchandise
    {
        public string MerchandiseId { get; set; }
        public string OwnerAccount { get; set; }
        public int Price { get; set; }
        public string MerchandiseTitle { get; set; }
        public string MerchandiseContent { get; set; }
        public int SoldQty { get; set; }
        public int RemainingQty { get; set; }
        public string CategoryId { get; set; }
        public string CategoryDetailId { get; set; }
        public string ImagePath { get; set; }
        public bool EnableSpec { get; set; }
    }
}
