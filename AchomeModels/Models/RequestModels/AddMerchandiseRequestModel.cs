using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AchomeModels.Models.RequestModels
{
    public class AddMerchandiseRequestModel
    {
        public int Price { get; set; }
        public string MerchandiseTitle { get; set; }
        public string MerchandiseContent { get; set; }
        public int RemainingQty { get; set; }
        public int CategoryId { get; set; }
        public int CategoryDetailId { get; set; }
        public IFormFile MerchandisePhotos { get; set; }

        public bool EnableSpec { get; set; }
        public List<AddSpecModel> SpecList { get; set; }
    }

    public class AddSpecModel
    {
        //public string MerchandiseId { get; set; }
        public int Price { get; set; }
        public int RemainingQty { get; set; }
        public string Spec1 { get; set; }
        public string Spec2 { get; set; }
        public bool Enable { get; set; }

    }
}
