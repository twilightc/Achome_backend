using System;
using System.Collections.Generic;

namespace AchomeModels.DbModels
{
    public partial class TransportMethod
    {
        public int TransportId { get; set; }
        public string TransportName { get; set; }
        public int Fee { get; set; }
    }
}
