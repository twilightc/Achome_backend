﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Achome.Models.RequestModels
{
    public class RemoveShoppingCartItemRequestModel
    {
        public string Account { get; set; }
        public string ProdId { get; set; }
        public int SpecId { get; set; }
    }
}