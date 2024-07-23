using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Affiliate_Shopee.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Affiliate_ShopeeUser class
public class Affiliate_ShopeeUser : IdentityUser
{
    [Required]
    [PersonalData]
    [Display(Name = "Name")]
    public string Name { get; set; }
    [Required]
    [PersonalData]
    [Display(Name = "Discriminator")]
    public string Discriminator { get; set; }
    [Required]
    [PersonalData]
    [Display(Name = "Status")]
    public string Status { get; set; }
    public string NameBank { get; set; }
    public string IdBank { get; set; }
    public string Otp { get; set; }
    public string FmcToken { get; set; }
}

