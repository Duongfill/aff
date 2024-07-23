using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class DealPaymentType
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public int PaymentTypeId { get; set; }

        public virtual Deal Deal { get; set; } = null!;
        public virtual PaymentType PaymentType { get; set; } = null!;
    }
}
