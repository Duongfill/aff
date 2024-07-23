using Affiliate_Shopee.Areas.Identity.Data;
using Affiliate_Shopee.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace Affiliate_Shopee.Areas.Identity.Pages.Account.Manage
{
    public class CustomerUserManager
    {
        private readonly UserManager<Affiliate_ShopeeUser> _userManager;

        public CustomerUserManager(UserManager<Affiliate_ShopeeUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Affiliate_ShopeeUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
