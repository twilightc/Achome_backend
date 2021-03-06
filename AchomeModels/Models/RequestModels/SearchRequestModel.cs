﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Models.RequestModels
{
    public enum SortTypeEnum
    {
        None,SoldQty,Price,Date
    }
    public enum OrderTypeEnum
    {
        None,Asc,Desc
    }
    public class SearchRequestModel
    {
        public string CategoryId { get; set; }
        public string CategoryDetailId { get; set; }

        public string Keyword { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public SortTypeEnum? SortType { get; set; }

        public OrderTypeEnum? OrderType { get; set; }
    }

    public class BackStageSearchModel
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string OrderString { get; set; }
        public bool isOrderByDesc { get; set; }
    }

}
