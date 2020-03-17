using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Models.ResponseModels
{

#pragma warning disable CA2227 // 集合屬性應為唯讀
    public class ShoppingCartWrapper
    {
        public string SellerAccount { get; set; }
        public List<ShoppingCartViewModel> ShoppingCartViewModels { get; set; }
    }
#pragma warning restore CA2227

    public class ShoppingCartViewModel
    {
        public string MerchandiseTitle { get; }
        public string OwnerAccount { get; }
        public string MerchandiseId { get; }
        public int? Price { get; }

        public int SpecId { get; set; }
        public string Spec1 { get; }
        public string Spec2 { get; }
        public int PurchaseQty { get; }

        public ShoppingCartViewModel(string merchandiseTitle, string ownerAccount, string merchandiseId, int? price, int sepcId, string spec1, string spec2, int purchaseQty)
        {
            MerchandiseTitle = merchandiseTitle;
            OwnerAccount = ownerAccount;
            MerchandiseId = merchandiseId;
            Price = price;
            SpecId = sepcId;
            Spec1 = spec1;
            Spec2 = spec2;
            PurchaseQty = purchaseQty;
        }

        public override bool Equals(object obj)
        {
            return obj is ShoppingCartViewModel other &&
                   MerchandiseTitle == other.MerchandiseTitle &&
                   OwnerAccount == other.OwnerAccount &&
                   MerchandiseId == other.MerchandiseId &&
                   Price == other.Price &&
                   SpecId == other.SpecId &&
                   Spec1 == other.Spec1 &&
                   Spec2 == other.Spec2 &&
                   PurchaseQty == other.PurchaseQty;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MerchandiseTitle, OwnerAccount, MerchandiseId, Price, SpecId, Spec1, Spec2, PurchaseQty);
        }
    }
}
