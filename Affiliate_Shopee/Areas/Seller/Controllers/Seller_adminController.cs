using Affiliate_Shopee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Affiliate_Shopee.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class Seller_adminController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ShopeeAffContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Seller_adminController(ShopeeAffContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var orderStats = _context.Orders
            .Where(o => o.UserId == userId && o.Status != "Mới")
            .GroupBy(o => o.Status)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count()
            }).ToList();
            var dealStats = _context.Deals
                .Where(d => d.UserId == userId)
                .GroupBy(d => d.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                }).ToList();
            ViewBag.OrderStats = orderStats;
            ViewBag.DealStats = dealStats;
            return View();
        }
    }
}
