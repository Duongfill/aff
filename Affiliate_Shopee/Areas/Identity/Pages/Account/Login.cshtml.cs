// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Affiliate_Shopee.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Google.Api;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using FirebaseAdmin.Auth;
using Affiliate_Shopee.Areas.Identity.Pages.Account.Manage;

namespace Affiliate_Shopee.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Affiliate_ShopeeUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CustomerUserManager _customUserManager;
        public LoginModel(SignInManager<Affiliate_ShopeeUser> signInManager, ILogger<LoginModel> logger, IHttpContextAccessor httpContextAccessor, CustomerUserManager customUserManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _customUserManager = customUserManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string ErrorMessage1 { get; set; }

        public class InputModel
        {
            
            public string UserName { get; set; }

            [Phone]
            public string PhoneNumber { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (string.IsNullOrEmpty(Input.PhoneNumber))
            {
                ErrorMessage = "Tên đăng nhập không được để trống.";
                ModelState.AddModelError("Input.PhoneNumber", "Số điện thoại không được để trống.");
                return Page();
            }
            else
            {
                ErrorMessage = "";
                //ModelState.AddModelError("Input.PhoneNumber", "");

            }

            if (string.IsNullOrEmpty(Input.Password))
            {
                ErrorMessage1 = "Mật khẩu không được để trống.";
                ModelState.AddModelError("Input.Password", "Mật khẩu không được để trống.");
                return Page();
            }
            else
            {
                ErrorMessage1 = "";
                //ModelState.AddModelError("Input.Password", "");

            }
            if (ModelState.IsValid)
            {
                var user = await _customUserManager.FindByPhoneNumberAsync(Input.PhoneNumber);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        _httpContextAccessor.HttpContext.Session.SetString("Id", user.Id);
                        _httpContextAccessor.HttpContext.Session.SetString("UserName", user.UserName);
                        Response.Cookies.Append("UserId", user.Id);
                        Response.Cookies.Append("UserName", user.UserName);
                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại!");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại!");

                    return Page();
                }
            }
            ModelState.AddModelError("Input.Password", "");
            ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại!");
            return Page();
        }
    }
}
