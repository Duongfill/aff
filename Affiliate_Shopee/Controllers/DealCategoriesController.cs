using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Affiliate_Shopee.Models;
using X.PagedList;

namespace Affiliate_Shopee.Controllers
{
    public class DealCategoriesController : Controller
    {
        private readonly ShopeeAffContext _context;

        public DealCategoriesController(ShopeeAffContext context)
        {
            _context = context;
        }

        // GET: DealCategories
        public async Task<IActionResult> Index()
        {
              return _context.DealCategories != null ? 
                          View(await _context.DealCategories.ToListAsync()) :
                          Problem("Entity set 'ShopeeAffContext.DealCategories'  is null.");
        }

        // GET: DealCategories/Details/5
        public async Task<IActionResult> Details(int? id, int? page)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;
            var dealCategory = await _context.DealCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (dealCategory == null)
            {
                return NotFound();
            }

            var pageNumber = page ?? 1; // Trang hiện tại, nếu không có trang nào được chọn thì mặc định là trang 1
            var pageSize = 18; // Số mục trên mỗi trang

            var deals = await _context.Deals.Where(d => d.CategoryId == id && d.Status == "Đang hoạt động" && d.StatusReason == 1).ToPagedListAsync(pageNumber, pageSize);
            ViewBag.DealCategoryName = dealCategory.Name;

            return View(deals);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status")] DealCategory dealCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dealCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dealCategory);
        }

        // GET: DealCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DealCategories == null)
            {
                return NotFound();
            }

            var dealCategory = await _context.DealCategories.FindAsync(id);
            if (dealCategory == null)
            {
                return NotFound();
            }
            return View(dealCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status")] DealCategory dealCategory)
        {
            if (id != dealCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dealCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DealCategoryExists(dealCategory.Id))
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
            return View(dealCategory);
        }

        // GET: DealCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DealCategories == null)
            {
                return NotFound();
            }

            var dealCategory = await _context.DealCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dealCategory == null)
            {
                return NotFound();
            }

            return View(dealCategory);
        }

        // POST: DealCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DealCategories == null)
            {
                return Problem("Entity set 'ShopeeAffContext.DealCategories'  is null.");
            }
            var dealCategory = await _context.DealCategories.FindAsync(id);
            if (dealCategory != null)
            {
                _context.DealCategories.Remove(dealCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DealCategoryExists(int id)
        {
          return (_context.DealCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
