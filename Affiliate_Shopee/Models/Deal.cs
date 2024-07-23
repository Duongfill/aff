using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class Deal
    {
        public Deal()
        {
            DealDeposits = new HashSet<DealDeposit>();
            DealPaymentTypes = new HashSet<DealPaymentType>();
            DealWithdraws = new HashSet<DealWithdraw>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShopeeLink { get; set; } = null!;
        public string? Code { get; set; }
        public int Slot { get; set; }
        public int? ShopeePrice { get; set; }
        public int? RefundPrice { get; set; }
        public int? IsFreeship { get; set; }
        public string? Location { get; set; }
        public int? DealExpired { get; set; }
        public DateTime? DealExpiredAt { get; set; }
        public int? CategoryId { get; set; }
        public int? GroupFacebookId { get; set; }
        public string? RefundType { get; set; }
        public int? Quantity { get; set; }
        public string? LinkFacebook { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedName { get; set; }
        public string? UpdatedName { get; set; }
        public string? Status { get; set; }
        public int? StatusReason { get; set; }
        public string? Note { get; set; }
        public string? UserId { get; set; }
        public string? ImageDeal { get; set; }
        public string? AffiliateLink { get; set; }

        public virtual DealCategory? Category { get; set; }
        public virtual GroupFacebook? GroupFacebook { get; set; }
        public virtual AspNetUser? User { get; set; }
        public virtual ICollection<DealDeposit> DealDeposits { get; set; }
        public virtual ICollection<DealPaymentType> DealPaymentTypes { get; set; }
        public virtual ICollection<DealWithdraw> DealWithdraws { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
