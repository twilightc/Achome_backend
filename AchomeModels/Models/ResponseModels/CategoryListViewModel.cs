using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchomeModels.Models.ResponseModels
{
    public class RawCategoryListViewModel
    {
        public string Cid { get; set; }
        public string Cname { get; set; }
        public int GroupSeq { get; set; }
        public string DetailId { get; set; }
        public string DetailName { get; set; }
        public int Seq { get; set; }
    }
    public class CategoryListViewModel
    {
        public string Cid { get; set; }
        public string Cname { get; set; }
        public int GroupSeq { get; set; }

        public List<CategoryListDetailViewModel> Detail { get; set; }
    }

    public  class CategoryListDetailViewModel
    {
        public string DetailId { get; set; }
        public string DetailName { get; set; }
        public int Seq { get; set; }

        
    }
}
