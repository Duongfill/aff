using Microsoft.AspNetCore.Mvc;

namespace Affiliate_Shopee.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
