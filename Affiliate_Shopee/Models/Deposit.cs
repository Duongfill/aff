using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class Deposit
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public string? UserId { get; set; }
        public int? TransactionId { get; set; }
        public string? PaymentId { get; set; }
        public string? Gateway { get; set; }
        public byte? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Transaction? Transaction { get; set; }
    }
}
