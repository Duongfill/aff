using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Affiliate_Shopee.Models;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace Affiliate_Shopee.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SupportAdmin")]
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

        public async Task<IActionResult> Index(int? page, string textsearch, string status, DateTime? createdat)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var query = _context.Deals.Include(d => d.User).AsQueryable();
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

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.DealCategories, "Id", "Id");
            ViewData["GroupFacebookId"] = new SelectList(_context.GroupFacebooks, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ShopeeLink,Code,Slot,ShopeePrice,RefundPrice,IsFreeship,Location,DealExpired,DealExpiredAt,CategoryId,GroupFacebookId,RefundType,Quantity,LinkFacebook,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy,CreatedName,UpdatedName,Status,StatusReason,Note,UserId,ImageDeal,AffiliateLink")] Deal deal)
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
        public async Task<IActionResult> Edit(int id, string status)
        {

            var deal = await _context.Deals.FindAsync(id);
            if (deal == null)
            {
                return NotFound();
            }

            deal.Status = status;
            deal.StatusReason = 0;
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
            return View(deal);
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

        // POST: Admin/Deals/Delete/5
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
