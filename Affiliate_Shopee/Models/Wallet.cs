using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class Wallet
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public decimal? OldAmount { get; set; }
        public string? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public byte? Status { get; set; }

        public virtual AspNetUser? User { get; set; }
    }
}
