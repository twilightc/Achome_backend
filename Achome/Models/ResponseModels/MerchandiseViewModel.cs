using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Models.ResponseModels
{
    public class MerchandiseWrapper
    {
        public List<MerchandiseViewModel> MerchandiseViewModel { get; set; }
        public int MerchandiseAmount { get; set; }
    }


    public class MerchandiseViewModel
    {
        public string MerchandiseId { get; set; }

        public string OwnerAccount { get; set; }

        public int Price { get; set; }
        public string MerchandiseTitle { get; set; }

        public string MerhandiseContent { get; set; }

        public int SoldQty { get; set; }

        public int RemainingQty { get; set; }
        public string CategoryId { get; set; }
        public string CategoryDetailId { get; set; }
        public string ImagePath { get; set; }

        public bool EnableSpec { get; set; }
#pragma warning disable CA2227 // 集合屬性應為唯讀
        public List<MerchandiseSpecViewModel> MerchandiseSpec { get; set; }

        public List<MerchandiseQaViewModel> MerchandiseQa { get; set; }
        public List<string> Spec1 { get; set; }
        public List<string> Spec2 { get; set; }
#pragma warning restore CA2227 // 集合屬性應為唯讀
    }

    public class MerchandiseSpecViewModel
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

    public class MerchandiseQaViewModel
    {
        public string MerchandiseId { get; set; }
        public int Seq { get; set; }
        public string QuestionAccount { get; set; }
        public string Question { get; set; }
        public DateTime AskingTime { get; set; }
        public string Answer { get; set; }
        public DateTime AnswerTime { get; set; }
    }

#pragma warning disable CA2227 // 集合屬性應為唯讀
    public class ShoppingCartWrapper
    {
        public string SellerAccount { get; set; }
        public  List<ShoppingCartViewModel> ShoppingCartViewModels { get; set; }
    }
#pragma warning restore CA2227

    public class ShoppingCartViewModel
    {
        public string MerchandiseTitle { get; }
        public string OwnerAccount { get; }
        public string MerchandiseId { get; }
        public int? Price { get; }
        public string Spec1 { get; }
        public string Spec2 { get; }
        public int PurchaseQty { get; }

        public ShoppingCartViewModel(string merchandiseTitle, string ownerAccount, string merchandiseId, int? price, string spec1, string spec2, int purchaseQty)
        {
            MerchandiseTitle = merchandiseTitle;
            OwnerAccount = ownerAccount;
            MerchandiseId = merchandiseId;
            Price = price;
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
                   Spec1 == other.Spec1 &&
                   Spec2 == other.Spec2 &&
                   PurchaseQty == other.PurchaseQty;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MerchandiseTitle, OwnerAccount, MerchandiseId, Price, Spec1, Spec2, PurchaseQty);
        }
    }
}
