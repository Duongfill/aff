using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class UserProfile
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Avartar { get; set; }
        public int? BankId { get; set; }
        public string? BankName { get; set; }
        public string? BankNumber { get; set; }
        public string? BankQrCode { get; set; }
        public string? UserId { get; set; }
        public byte? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Bank? Bank { get; set; }
        public virtual AspNetUser? User { get; set; }
    }
}
