using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class Complain
    {
        public int Id { get; set; }
        public string? Userid { get; set; }
        public int? Orderid { get; set; }
        public DateTime? Createdat { get; set; }
        public string? Descriptions { get; set; }
        public string? Status { get; set; }

        public virtual Order? Order { get; set; }
    }
}
