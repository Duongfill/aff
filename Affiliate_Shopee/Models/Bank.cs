using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class Bank
    {
        public Bank()
        {
            UserProfiles = new HashSet<UserProfile>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public byte? Status { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
