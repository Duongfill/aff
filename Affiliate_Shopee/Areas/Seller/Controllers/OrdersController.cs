using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Affiliate_Shopee.Models;
using X.PagedList;
using System.Diagnostics;
using Affiliate_Shopee.Controllers;
using Microsoft.Extensions.Logging;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;
using Google.Rpc;
using Microsoft.AspNetCore.Authorization;

namespace Affiliate_Shopee.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class OrdersController : Controller
    {
        private readonly ShopeeAffContext _context;
        private readonly ILogger<OrdersController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrdersController(ShopeeAffContext context, ILogger<OrdersController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public class OrderViewModel
        {
            public int OrderId { get; set; }
            public int DealId { get; set; }
            public string Code { get; set; }
            public string DealName { get; set; }
            public string Status { get; set; }
            public DateTime? CreatedAt { get; set; }
            public string UserName { get; set; }
            public string NameBank { get; set; }
            public string IdBank { get; set; }
            public string ImageOrder { get; set; }
            
        }

        // GET: Orders
        public IActionResult Index(int? page, string textsearch, string? status, DateTime? createdat, DateTime? todate, string bankname)
        {
            var pageNumber = page ?? 1;
            var pageSize = 30;
            var query = _context.Orders.Include(o => o.Deal).AsQueryable();
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            query = query.Where(o => o.Deal.UserId == userId);
            query = query.Where(o => o.Status == "Từ chối đánh giá" || o.Status == "Chờ hoàn tiền" || o.Status == "Đã nhận tiền" || o.Status == "Đã hoàn tiền" || o.Status == "Chờ duyệt");
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status == status);
            }
            if (!string.IsNullOrEmpty(textsearch))
            {
                query = query.Where(o => o.Deal.Code == textsearch);
            }
            if (createdat.HasValue && todate.HasValue)
            {
                query = query.Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date >= createdat.Value.Date && o.CreatedAt.Value.Date <= todate.Value.Date);
            }
            else if (createdat.HasValue)
            {
                query = query.Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date >= createdat.Value.Date);
            }
            else if (todate.HasValue)
            {
                query = query.Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date <= todate.Value.Date);
            }
            if (!string.IsNullOrEmpty(bankname))
            {
                query = query.Where(o => _context.AspNetUsers.Any(u => u.Id == o.UserId && u.NameBank == bankname));
            }
            query = query.OrderByDescending(o => o.CreatedAt);
            var ordersWithUserNames = query
                .Select(o => new OrderViewModel
                {
                    DealName = o.Deal.Name,
                    OrderId = o.Id,
                    DealId = o.Deal.Id,
                    Code = o.Deal.Code,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    UserName = _context.AspNetUsers.Any(u => u.Id == o.UserId) ? _context.AspNetUsers.First(u => u.Id == o.UserId).Name : null,
                    NameBank = _context.AspNetUsers.Any(u => u.Id == o.UserId) ? _context.AspNetUsers.First(u => u.Id == o.UserId).NameBank : null,
                    IdBank = _context.AspNetUsers.Any(u => u.Id == o.UserId) ? _context.AspNetUsers.First(u => u.Id == o.UserId).IdBank : null
                })
                .ToPagedList(pageNumber, pageSize);
            ViewBag.Status = status;
            ViewBag.CreatedAt = createdat;
            ViewBag.ToDate = todate;
            ViewBag.BankName = bankname;
            ViewBag.TextSearch = textsearch;
            return View(ordersWithUserNames);
        }
       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Deal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public IActionResult Create()
        {
            ViewData["DealId"] = new SelectList(_context.Deals, "Id", "Id");
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
               .Include(o => o.Deal)
               .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            var user = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Id == order.UserId);
            var ImageOrder = await _context.OrderImages.FirstOrDefaultAsync(io => io.OrderId == order.Id);
            if (user != null)
            {
                ViewBag.NameBank = user.NameBank;
                ViewBag.IdBank = user.IdBank;
                ViewBag.NameUser = user.Name;
                
            }
            else
            {
                ViewBag.NameBank = null;
                ViewBag.IdBank = null;
                ViewBag.NameUser = null;
            }
            if (ImageOrder != null)
            {
                ViewBag.Name = ImageOrder.LinkUrl2;

            }
            else
            {
                ViewBag.Name = null;

            }
            ViewBag.DealName = order.Deal != null ? order.Deal.Name : null;
            ViewBag.RefundPrice = order.PaymentTypeId;
            ViewData["DealId"] = new SelectList(_context.Deals, "Id", "Id", order.DealId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string? Status, string? sellerfeedback)
        {
            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            if (id != existingOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingOrder.Status = Status;
                    existingOrder.SellerFeedback = string.IsNullOrEmpty(sellerfeedback) ? null : sellerfeedback;
                    existingOrder.UpdatedAt =  DateTime.Now;
                    _context.Update(existingOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(existingOrder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DealId"] = new SelectList(_context.Deals, "Id", "Id", existingOrder.DealId);
            return View(existingOrder);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Deal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ShopeeAffContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
