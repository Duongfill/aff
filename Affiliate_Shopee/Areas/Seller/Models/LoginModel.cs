using System.ComponentModel.DataAnnotations;

namespace Affiliate_Shopee.Areas.Seller.Models
{
    public class LoginModel
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
