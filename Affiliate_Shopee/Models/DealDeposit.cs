using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class DealDeposit
    {
        public int Id { get; set; }
        public int? DealId { get; set; }
        public decimal? TotalCashbackAmount { get; set; }
        public decimal? RefundPrice { get; set; }
        public decimal? Amount { get; set; }
        public int? Slot { get; set; }
        public byte? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? TransacitonId { get; set; }

        public virtual Deal? Deal { get; set; }
        public virtual Transaction? Transaciton { get; set; }
    }
}
