using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class PaymentType
    {
        public PaymentType()
        {
            DealPaymentTypes = new HashSet<DealPaymentType>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<DealPaymentType> DealPaymentTypes { get; set; }
    }
}
