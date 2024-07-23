using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class Seller
    {
        public int Id { get; set; }
        public string? NameShop { get; set; }
        public string? Email { get; set; }
        public string? Linkfb { get; set; }
        public string? Img { get; set; }
        public string? Userid { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual AspNetUser? User { get; set; }
    }
}
