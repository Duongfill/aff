using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class OrderStatus
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
