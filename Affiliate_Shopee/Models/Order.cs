using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class Order
    {
        public Order()
        {
            Complains = new HashSet<Complain>();
            OrderHistories = new HashSet<OrderHistory>();
            OrderImages = new HashSet<OrderImage>();
        }

        public int Id { get; set; }
        public int? DealId { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserId { get; set; }
        public int? PaymentTypeId { get; set; }
        public DateTime? UpdatedAt2 { get; set; }
        public DateTime? UpdatedAt3 { get; set; }
        public string? SellerFeedback { get; set; }
        public string? BuyerFeedback { get; set; }

        public virtual Deal? Deal { get; set; }
        public virtual AspNetUser? User { get; set; }
        public virtual ICollection<Complain> Complains { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
        public virtual ICollection<OrderImage> OrderImages { get; set; }
    }
}
