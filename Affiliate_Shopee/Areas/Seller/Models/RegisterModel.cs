using System.ComponentModel.DataAnnotations;

namespace Affiliate_Shopee.Areas.Seller.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Số điện thoại")]
        [StringLength(100, ErrorMessage = "{0} không được dài quá 100 ký tự.")]
        public string PhoneNumber { get; set; }
    }
}
