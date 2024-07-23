using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class Withdraw
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public int Type { get; set; }
        public int? TransactionId { get; set; }
        public string? UserId { get; set; }
        public int? BankId { get; set; }
        public int? BankNumber { get; set; }
        public string? BankName { get; set; }
        public string? QrCode { get; set; }
        public byte? Status { get; set; }
        public decimal? Balance { get; set; }
        public decimal? OldBlance { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Transaction? Transaction { get; set; }
    }
}
