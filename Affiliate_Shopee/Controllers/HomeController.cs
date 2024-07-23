using Affiliate_Shopee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Affiliate_Shopee.Areas.Identity.Data;
using Affiliate_Shopee.Areas.Identity.Pages.Account.Manage;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace Affiliate_Shopee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopeeAffContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Affiliate_ShopeeUser> _userManager;
        private readonly SignInManager<Affiliate_ShopeeUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, ShopeeAffContext context, IHttpContextAccessor httpContextAccessor, UserManager<Affiliate_ShopeeUser> userManager, SignInManager<Affiliate_ShopeeUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index(int? page)
        {
            UpdateDealStatus();
            var pageNumber = page ?? 1;
            var pageSize = 30;
            var deals = _context.Deals.Where(d => d.StatusReason == 1 && d.Status 
            != "Hết hạn" && d.Status != "Chờ duyệt").ToPagedList(pageNumber, pageSize);
            return View(deals);
        }

        public async Task<IActionResult> Complain()
        {
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var complaints = await _context.Complains
                .Where(c => c.Userid == userId)
                .Include(c => c.Order)
                    .ThenInclude(o => o.User)
                .Include(c => c.Order)
                    .ThenInclude(o => o.Deal)
                .OrderByDescending(c => c.Createdat)
                .ToListAsync();

            return View(complaints);
        }

        public void UpdateDealStatus()
        {
            var dealsToExpire = _context.Deals.Where(d => d.DealExpiredAt <= DateTime.Now && d.Status != "Hết hạn").ToList();
            foreach (var deal in dealsToExpire)
            {
                deal.Status = "Hết hạn";
                _context.Update(deal);
            }
            _context.SaveChanges();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                _logger.LogError("Query parameter is required");
                return BadRequest("Query parameter is required");
            }

            _logger.LogInformation($"Searching for deals with query: {query}");
            var normalizedQuery = RemoveDiacritics(query.ToLower());
            var results = _context.Deals
                .Where(d => d.StatusReason == 1 && d.Status != "Chờ duyệt" && d.Status != "Hết hạn")
                .AsEnumerable()
                .Where(d => RemoveDiacritics(d.Name.ToLower()).IndexOf(normalizedQuery) >= 0)
                .Select(d => new { d.Id, d.Name })
                .Take(5)
                .ToList();

            _logger.LogInformation($"Found {results.Count} deals matching the query.");
            return Ok(results);
        }

        private string RemoveDiacritics(string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            ).Normalize(NormalizationForm.FormC);
        }

        [HttpGet]
        public IActionResult SearchAll(string query, int? page)
        {
            if (string.IsNullOrEmpty(query))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập từ khóa tìm kiếm!";
                return RedirectToAction("Index");
            }

            ViewBag.Query = query; // Lưu trữ giá trị query vào ViewBag
            var pageNumber = page ?? 1;
            var pageSize = 30;
            var queryResults = _context.Deals
                .Where(d => d.StatusReason == 1 && d.Name.Contains(query) && d.Status != "Chờ duyệt" && d.Status != "Hết hạn");
            var totalItems = queryResults.Count();
            var pagedResults = queryResults
                .OrderBy(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagedResultList = new StaticPagedList<Deal>(pagedResults, pageNumber, pageSize, totalItems);
            return View("SearchAll", pagedResultList);
        }

        public async Task<IActionResult> RedirectToSeller()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                var returnUrl = Url.Action("RedirectToSeller", "Home", new { area = "" });
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                var returnUrl = Url.Action("RedirectToSeller", "Home", new { area = "" });
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            bool hasShop = _context.Sellers.Any(s => s.Userid == user.Id);
            if (hasShop)
            {
                return RedirectToAction("Details", "Sellers", new { area = "Seller" });
            }
            else
            {
                return RedirectToAction("Create", "Sellers");
            }
        }

    }
}
