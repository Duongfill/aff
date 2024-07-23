using Affiliate_Shopee.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Affiliate_Shopee.Areas.Identity.Pages.Account
{
    public class VerifyOtpModel : PageModel
    {
        private readonly UserManager<Affiliate_ShopeeUser> _userManager;
        private readonly SignInManager<Affiliate_ShopeeUser> _signInManager;

        public VerifyOtpModel(UserManager<Affiliate_ShopeeUser> userManager, SignInManager<Affiliate_ShopeeUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Email { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "OTP Code")]
            public string OtpCode { get; set; }
        }

        public void OnGet(string email)
        {
            Email = email;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var email = TempData["Email"] as string;
                if (email != null)
                {
                    // Retrieve the user by email
                    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
                    if (user != null)
                    {
                        // Check if OTP is not null
                        if (!string.IsNullOrEmpty(user.Otp) && user.Otp == Input.OtpCode)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToPage("/Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid OTP.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not available.");
                }
            }
            return Page();
        }
    }
}
