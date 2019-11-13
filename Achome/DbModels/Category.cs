using System;
using System.Collections.Generic;

namespace Achome.DbModels
{
    public partial class Category
    {
        public string Cid { get; set; }
        public string Cname { get; set; }
        public int GroupSeq { get; set; }
    }
}
