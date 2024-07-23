using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Affiliate_Shopee.Models;
using Order = Affiliate_Shopee.Models.Order;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using Affiliate_Shopee.Services;

namespace Affiliate_Shopee.Controllers
{
    public class DealsController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ShopeeAffContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DealsController(ShopeeAffContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;

        }
        public async Task<IActionResult> Index()
        {
            var shopeeAffContext = _context.Deals.Include(d => d.Category).Include(d => d.GroupFacebook).Include(d => d.User);
            return View(await shopeeAffContext.ToListAsync());
        }
        public IActionResult Details(int id)
        {
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var deal = _context.Deals.FirstOrDefault(p => p.Id == id);
            if (deal == null)
            {
                return RedirectToAction("Error", "Home");
            }
            string status = null;
            string sellerFeedback = null;
            if (!string.IsNullOrEmpty(userId))
            {
                var order = _context.Orders.FirstOrDefault(o => o.DealId == id && o.UserId == userId && o.Status != "Đã nhận tiền");
                status = order != null ? order.Status : null;
                sellerFeedback = order != null ? order.SellerFeedback : null;
            }
            ViewBag.Status = status;
            ViewBag.SellerFeedback = sellerFeedback;

            var similarDeals = _context.Deals
                                 .Where(d => d.CategoryId == deal.CategoryId && d.Id != id && d.Status == "Đang hoạt động" && d.StatusReason == 1).Take(6)
                                 .ToList();
            ViewBag.SimilarDeals = similarDeals;

            return View(deal);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(int Id)
        {
            var createdat = DateTime.Now;
            string loginUrl = Url.Action("Login", "Account", new { area = "Identity" });
            var status = "Đợi mua hàng";
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            if (userId == null)
            {
                
                return Redirect(loginUrl);
            }
            var deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == Id);
            if (deal == null)
            {
                return Json(new { success = false, message = "Không tìm thấy deal." });
            }
            if (deal.Slot <= 0)
            {
                return Json(new { success = false, message = "Deal này đã hết hạn rồi, không thể mua." });
            }
            deal.Slot -= 1;
            _context.Deals.Update(deal);
            var RefundPrice = deal.RefundPrice;
            var order = new Order
            {
                DealId = Id,
                CreatedAt = createdat,
                Status = status,
                UserId = userId,
                PaymentTypeId = int.Parse(RefundPrice.ToString())
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _httpContextAccessor.HttpContext.Session.SetInt32("OrderId", order.Id);
            var shopeeLink = _context.Deals.FirstOrDefault(d => d.Id == Id)?.ShopeeLink;
            return Json(new { success = true, shopeeLink, createdat, status });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Feedback(int Id, string text)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
                var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.DealId == Id && o.UserId == userId);
                if (existingOrder != null)
                {
                    var complain = new Complain
                    {
                        Userid = userId,
                        Orderid = existingOrder.Id,
                        Descriptions = text,
                        Createdat = DateTime.Now,
                        Status = "Chưa xử lý"
                    };
                    _context.Complains.Add(complain);
                    await _context.SaveChangesAsync();
                    return Redirect("/Home/Complain");
                }
                else
                {
                    return Redirect("/Home/Complain");
                }
            }
            catch (Exception ex)
            {
                return Redirect("/Home/Complain");

            }
        }
        public void UpdateDealStatus()
        {
            var dealsToExpire = _context.Deals.Where(d => d.DealExpiredAt <= DateTime.Now && d.Status != "Hết hạn" || d.Slot ==0).ToList();
            foreach (var deal in dealsToExpire)
            {
                deal.Status = "Hết hạn";
                _context.Update(deal);
            }

            _context.SaveChanges();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int Id)
        {
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.DealId == Id && o.UserId == userId && o.Status == "Đã hoàn tiền");
            if (existingOrder != null)
            {
                existingOrder.Status = "Đã nhận tiền";
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Deals", new { id = Id });
            }
            return RedirectToAction("Details", "Deals", new { id = Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateImageB2(int Id, string LinkName, IFormFile inputfile2)
        {
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.DealId == Id && o.UserId == userId && o.Status != "Đã nhận tiền");
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "image");
            var uniqueFileName = LinkName;
            var status = "Chờ Review";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            order.Status = status;
            order.UpdatedAt2 = DateTime.Now;
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await inputfile2.CopyToAsync(stream);
            }
            var orderImage = new OrderImage
            {
                OrderId = order.Id,
                LinkUrl = LinkName,

            };
            _context.OrderImages.Add(orderImage);
            await _context.SaveChangesAsync();
            ViewBag.LinkName = LinkName;
            return RedirectToAction("Details", "Deals", new { id = Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateImageB3(int Id, string LinkName, IFormFile inputfile2)
        {
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.DealId == Id && o.UserId == userId && o.Status != "Đã nhận tiền");
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "image");
            var uniqueFileName = LinkName;
            var status = "Chờ duyệt";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            order.Status = status;
            order.UpdatedAt3 = DateTime.Now;
            var existingOrderImage = await _context.OrderImages.FirstOrDefaultAsync(oi => oi.OrderId == order.Id);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await inputfile2.CopyToAsync(stream);
            }
            if (existingOrderImage != null)
            {
                existingOrderImage.LinkUrl2 = LinkName;
            }
            await _context.SaveChangesAsync();
            ViewBag.LinkName = LinkName;
            return RedirectToAction("Details", "Deals", new { id = Id });
        }
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.DealCategories, "Id", "Id");
            ViewData["GroupFacebookId"] = new SelectList(_context.GroupFacebooks, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ShopeeLink,Code,Slot,ShopeePrice,RefundPrice,IsFreeship,Location,DealExpired,DealExpiredAt,CategoryId,GroupFacebookId,RefundType,Quantity,LinkFacebook,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,CreatedName,UpdatedName,Status,StatusReason,Note,UserId")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.DealCategories, "Id", "Id", deal.CategoryId);
            ViewData["GroupFacebookId"] = new SelectList(_context.GroupFacebooks, "Id", "Id", deal.GroupFacebookId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", deal.UserId);
            return View(deal);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Deals == null)
            {
                return NotFound();
            }

            var deal = await _context.Deals.FindAsync(id);
            if (deal == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.DealCategories, "Id", "Id", deal.CategoryId);
            ViewData["GroupFacebookId"] = new SelectList(_context.GroupFacebooks, "Id", "Id", deal.GroupFacebookId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", deal.UserId);
            return View(deal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ShopeeLink,Code,Slot,ShopeePrice,RefundPrice,IsFreeship,Location,DealExpired,DealExpiredAt,CategoryId,GroupFacebookId,RefundType,Quantity,LinkFacebook,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,CreatedName,UpdatedName,Status,StatusReason,Note,UserId")] Deal deal)
        {
            if (id != deal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DealExists(deal.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.DealCategories, "Id", "Id", deal.CategoryId);
            ViewData["GroupFacebookId"] = new SelectList(_context.GroupFacebooks, "Id", "Id", deal.GroupFacebookId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", deal.UserId);
            return View(deal);
        }

        // GET: Deals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Deals == null)
            {
                return NotFound();
            }

            var deal = await _context.Deals
                .Include(d => d.Category)
                .Include(d => d.GroupFacebook)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deal == null)
            {
                return NotFound();
            }

            return View(deal);
        }

        // POST: Deals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Deals == null)
            {
                return Problem("Entity set 'ShopeeAffContext.Deals'  is null.");
            }
            var deal = await _context.Deals.FindAsync(id);
            if (deal != null)
            {
                _context.Deals.Remove(deal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DealExists(int id)
        {
            return (_context.Deals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
