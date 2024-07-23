using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class DealCategory
    {
        public DealCategory()
        {
            Deals = new HashSet<Deal>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Deal> Deals { get; set; }
    }
}
