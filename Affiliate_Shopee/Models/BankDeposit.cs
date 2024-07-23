using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class BankDeposit
    {
        public int Id { get; set; }
        public string? Gateway { get; set; }
        public string? PaymentId { get; set; }
        public int? DepositId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
