using Affiliate_Shopee.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Affiliate_Shopee.Controllers
{
    [ApiController]
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Affiliate_ShopeeUser> _userManager;

        public AccountController(UserManager<Affiliate_ShopeeUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("SaveToken")]
        public async Task<IActionResult> SaveToken([FromBody] SaveTokenRequest request)
        {
            var user = await _userManager.FindByIdAsync(User.Identity.Name); // Assuming User.Identity.Name holds the user ID
            if (user == null)
            {
                return Unauthorized();
            }

            user.Otp = request.Token;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }

    public class SaveTokenRequest
    {
        public string Token { get; set; }
    }

}
