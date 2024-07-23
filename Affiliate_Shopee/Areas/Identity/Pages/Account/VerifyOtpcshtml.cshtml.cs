using Affiliate_Shopee.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Affiliate_Shopee.Areas.Identity.Pages.Account
{
    public class VerifyOtpcshtmlModel : PageModel
    {
        private readonly UserManager<Affiliate_ShopeeUser> _userManager;
        private readonly SignInManager<Affiliate_ShopeeUser> _signInManager;

        public VerifyOtpcshtmlModel(UserManager<Affiliate_ShopeeUser> userManager, SignInManager<Affiliate_ShopeeUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public VerifyOtpViewModel Input { get; set; }

        public class VerifyOtpViewModel
        {
            public string PhoneNumber { get; set; }
            public string Otp { get; set; }
        }

        public void OnGet(string phoneNumber)
        {
            Input = new VerifyOtpViewModel { PhoneNumber = phoneNumber };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.PhoneNumber);
                if (user != null && user.Otp == Input.Otp)
                {
                    user.Otp = null; // Clear OTP after verification
                    await _userManager.UpdateAsync(user);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError(string.Empty, "Invalid OTP.");
            }

            return Page();
        }
    }
}
