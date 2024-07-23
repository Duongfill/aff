using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Affiliate_Shopee.Models;
using static Google.Rpc.Help.Types;
using System.Web.Helpers;
using System.Text.RegularExpressions;
using Google.Protobuf.WellKnownTypes;
using X.PagedList;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Authorization;

namespace Affiliate_Shopee.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
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

        // GET: Seller/Deals
        public async Task<IActionResult> Index(int? page, string textsearch, string status, DateTime? createdat)
        {
            UpdateDealStatus();
            var pageNumber = page ?? 1;
            var pageSize = 30;
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var query = _context.Deals.Where(d => d.UserId == userId).AsQueryable();

            if (!string.IsNullOrEmpty(textsearch))
            {
                query = query.Where(d => d.Name.Contains(textsearch) || d.Code.Contains(textsearch));
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(d => d.Status == status);
            }
            if (createdat.HasValue)
            {
                query = query.Where(d => d.CreatedAt.Value.Date == createdat.Value.Date);
            }
            query = query.OrderByDescending(d => d.CreatedAt);
            var deals = await query.ToPagedListAsync(pageNumber, pageSize);
            ViewBag.TextSearch = textsearch;
            ViewBag.Status = status;
            ViewBag.CreatedAt = createdat;
            return View(deals);
        }
        public async Task<IActionResult> Details(int? id)
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
        [HttpPost]
        public IActionResult UpdateStatusReason(int id, int status)
        {
            var item = _context.Deals.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                int statusReasonValue = status;
                item.StatusReason = statusReasonValue;
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        public void UpdateDealStatus()
        {
            var dealsToExpire = _context.Deals.Where(d => d.DealExpiredAt <= DateTime.Now && d.Status != "Hết hạn" || d.Slot == 0).ToList();
            foreach (var deal in dealsToExpire)
            {
                deal.Status = "Hết hạn";
                deal.StatusReason = 0;
                _context.Update(deal);
            }

            _context.SaveChanges();
        }
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.DealCategories, "Id", "Name");
            ViewData["GroupFacebookId"] = new SelectList(_context.GroupFacebooks, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Nameimg, string Name, string shopeelink, int slot, int shopeeprice,
            int refundprice, int categoryid, string? note, int isfreeship, string location,
            int dealexpired, DateTime? dealexpiredat, IFormFile ImgDeal)
        {
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "image/imagesellerdeals");
            var uniqueFileName = Nameimg;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var checkuserid = await _context.Sellers.FirstOrDefaultAsync(s => s.Userid == userId);
            if (string.IsNullOrEmpty(Nameimg))
            {
                return Json(new { success = false, message = "Vui lòng tải lên hình ảnh." });
            }

            if (string.IsNullOrEmpty(Name))
            {
                return Json(new { success = false, message = "Vui lòng nhập tên sản phẩm." });
            }
            if (dealexpired == 0 && dealexpiredat < DateTime.Now)
            {
                return Json(new { success = false, message = "Thời gian hết hạn phải lớn hơn thời gian hiện tại." });
            }
            if (string.IsNullOrEmpty(shopeelink))
            {
                return Json(new { success = false, message = "Vui lòng nhập liên kết sản phẩm trên Shopee." });
            }

            if (slot < 10)
            {
                return Json(new { success = false, message = "Số lượng slot phải lớn hơn hoặc bằng 10." });
            }

            if (shopeeprice <= 0)
            {
                return Json(new { success = false, message = "Giá trên Shopee phải lớn hơn 0." });
            }

            if (refundprice <= 0)
            {
                return Json(new { success = false, message = "Số tiền hoàn lại phải lớn hơn 0." });
            }
            var name = Regex.Replace(Name.Trim(), @"\s+", " ");
            var existingDealWithName = await _context.Deals.FirstOrDefaultAsync(d => d.Name == name);
            var existingDealWithShopeeLink = await _context.Deals.FirstOrDefaultAsync(d => d.ShopeeLink == shopeelink);
            if (existingDealWithName != null)
            {
                return Json(new { success = false, message = "Tên sản phẩm đã được đặt trước đó." });
            }            
            if (name.Length <= 4 || name.Length >= 100)
            {
                return Json(new { success = false, message = "Tên sản phẩm phải có 4 đến 100 ký tự." });
            }
            if (!shopeelink.Contains("https://shopee.vn/"))
            {
                return Json(new { success = false, message = "Link Shopee không hợp lệ." });
            }
            if (refundprice > shopeeprice)
            {
                return Json(new { success = false, message = "Giá bán phải lớn hơn giá hoàn tiền." });
            }
            if (refundprice < 0.25 * shopeeprice)
            {
                return Json(new { success = false, message = "Giá hoàn tiền phải tối thiểu bằng 25% của giá trên Shopee." });
            }
            if (existingDealWithShopeeLink != null)
            {
                return Json(new { success = false, message = "Link Shopee đã tồn tại trong một deal khác." });
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImgDeal.CopyToAsync(stream);
            }
            var createdat = DateTime.Now;
            var createdatnow = DateTime.Now;
            var code = "D" + createdat.ToString("ddMMyyHHmmss");
            var status = "Chờ duyệt";
            int dealexpireda = dealexpired;
            var dealexpiredata = dealexpiredat;
            int statusreason = 1;
            if (dealexpiredat == null)
            {
                dealexpired = dealexpireda;
            }
            var deal = new Deal
            {
                UserId = userId,
                ImageDeal = Nameimg,
                Name = name,
                Slot = slot,
                ShopeeLink = shopeelink,
                CreatedAt = createdat,
                Location = location,
                RefundPrice = refundprice,
                ShopeePrice = shopeeprice,
                CategoryId = categoryid,
                IsFreeship = isfreeship,
                Note = note,
                Code = code,
                Status = status,
                StatusReason = statusreason,
                DealExpired = dealexpireda,
                DealExpiredAt = dealexpiredat,
            };
            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Deal đã được tạo thành công!";
            return Json(new { success = true });
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
            ViewData["CategoryId"] = new SelectList(_context.DealCategories, "Id", "Name", deal.CategoryId);
            ViewData["GroupFacebookId"] = new SelectList(_context.GroupFacebooks, "Id", "Id", deal.GroupFacebookId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", deal.UserId);
            return View(deal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string? Nameimg, [Bind("Id,Name,ShopeeLink,Slot,ShopeePrice,RefundPrice,IsFreeship,Location,DealExpired,DealExpiredAt,CategoryId")] Deal deal, IFormFile? ImgDeal)
        {
            if (id != deal.Id)
            {
                return Json(new { success = false, message = "Id của deal không hợp lệ." });
            }

            var existingDeal = await _context.Deals.FindAsync(id);
            if (existingDeal == null)
            {
                return Json(new { success = false, message = "Deal không tồn tại." });
            }

            var existingDealWithName = await _context.Deals.FirstOrDefaultAsync(d => d.Name == deal.Name.Trim() && d.Id != id);
            if (existingDealWithName != null)
            {
                return Json(new { success = false, message = "Tên sản phẩm đã được đặt trước đó." });
            }

            var existingDealWithShopeeLink = await _context.Deals.FirstOrDefaultAsync(d => d.ShopeeLink == deal.ShopeeLink.Trim() && d.Id != id);
            if (existingDealWithShopeeLink != null)
            {
                return Json(new { success = false, message = "Trùng link sản phẩm với deal khác. Vui lòng không đăng deal nhiều lần." });
            }

            if (string.IsNullOrEmpty(deal.Name))
            {
                return Json(new { success = false, message = "Tên sản phẩm không được để trống." });
            }
            if (string.IsNullOrEmpty(deal.ShopeeLink))
            {
                return Json(new { success = false, message = "Link sản phẩm trên Shopee không được để trống." });
            }
            if (deal.ShopeePrice <= 0 || deal.ShopeePrice == null)
            {
                return Json(new { success = false, message = "Giá trên Shopee phải tối thiểu là  1.000 vnđ." });
            }
            if (deal.RefundPrice <= 0)
            {
                return Json(new { success = false, message = "Giá hoàn tiền phải lớn hơn 0." });
            }
            if (deal.RefundPrice > deal.ShopeePrice)
            {
                return Json(new { success = false, message = "Giá bán phải lớn hơn giá hoàn tiền." });
            }
            if (deal.RefundPrice < 0.25 * deal.ShopeePrice)
            {
                return Json(new { success = false, message = "Giá hoàn tiền phải tối thiểu bằng 25% của giá trên Shopee." });
            }
            if (deal.DealExpired == 0 && deal.DealExpiredAt <= DateTime.Now)
            {
                return Json(new { success = false, message = "Thời gian hết hạn phải lớn hơn thời gian hiện tại." });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (ImgDeal != null)
                    {
                        var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "image/imagesellerdeals");
                        var uniqueFileName = Nameimg;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImgDeal.CopyToAsync(stream);
                        }
                        existingDeal.ImageDeal = uniqueFileName;
                    }
                    existingDeal.Name = deal.Name.Trim();
                    existingDeal.ShopeeLink = deal.ShopeeLink;
                    existingDeal.ShopeePrice = deal.ShopeePrice;
                    existingDeal.RefundPrice = deal.RefundPrice;
                    existingDeal.Slot = deal.Slot;
                    existingDeal.Location = deal.Location;
                    existingDeal.DealExpired = deal.DealExpired;
                    existingDeal.DealExpiredAt = deal.DealExpiredAt;
                    existingDeal.CategoryId = deal.CategoryId;
                    existingDeal.IsFreeship = deal.IsFreeship;
                    existingDeal.UpdatedAt = DateTime.Now;
                    existingDeal.Status = "Chờ duyệt";
                    _context.Update(existingDeal);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Deal đã được cập nhật thành công!" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DealExists(deal.Id))
                    {
                        return Json(new { success = false, message = "Deal không tồn tại." });
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Nếu có lỗi, hiển thị lại form với thông báo lỗi
            ViewData["CategoryId"] = new SelectList(_context.DealCategories, "Id", "Name", deal.CategoryId);
            ViewData["GroupFacebookId"] = new SelectList(_context.GroupFacebooks, "Id", "Id", deal.GroupFacebookId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", deal.UserId);
            return Json(new { success = false, message = "Dữ liệu đầu vào không hợp lệ." });
        }


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

        // POST: Seller/Deals/Delete/5
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
