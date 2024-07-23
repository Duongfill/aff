using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class OrderImage
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? OrderId { get; set; }
        public string LinkUrl { get; set; } = null!;
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? LinkUrl2 { get; set; }

        public virtual Order? Order { get; set; }
    }
}
