using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Models.RequestModels
{
    public class PendedShoppingCartItemRequestModel
    {
        public string Account { get; set; }
        public string ProdId { get; set; }
        public int SpecId { get; set; }
    }
}
