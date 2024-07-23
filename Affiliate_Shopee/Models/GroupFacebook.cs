using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class GroupFacebook
    {
        public GroupFacebook()
        {
            Deals = new HashSet<Deal>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public byte? Status { get; set; }

        public virtual ICollection<Deal> Deals { get; set; }
    }
}
