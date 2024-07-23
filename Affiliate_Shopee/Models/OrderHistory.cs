using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class OrderHistory
    {
        public int Id { get; set; }
        public DateTime? Time { get; set; }
        public string? Action { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? OrderId { get; set; }

        public virtual Order? Order { get; set; }
    }
}
