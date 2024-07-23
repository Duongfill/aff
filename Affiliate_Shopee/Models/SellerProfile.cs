using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class SellerProfile
    {
        public int Id { get; set; }
        public string? Fullname { get; set; }
        public string? LinkFacebook { get; set; }
        public string? ImageUrl { get; set; }
        public byte? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? Updatedby { get; set; }
        public string? UserId { get; set; }

        public virtual AspNetUser? User { get; set; }
    }
}
