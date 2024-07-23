using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Affiliate_Shopee.Models;

namespace Affiliate_Shopee.Controllers
{
    public class OrderImagesController : Controller
    {
        private readonly ShopeeAffContext _context;

        public OrderImagesController(ShopeeAffContext context)
        {
            _context = context;
        }

        // GET: OrderImages
        public async Task<IActionResult> Index()
        {
            var shopeeAffContext = _context.OrderImages.Include(o => o.Order);
            return View(await shopeeAffContext.ToListAsync());
        }

        // GET: OrderImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderImages == null)
            {
                return NotFound();
            }

            var orderImage = await _context.OrderImages
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderImage == null)
            {
                return NotFound();
            }

            return View(orderImage);
        }

        // GET: OrderImages/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OrderId,LinkUrl,Status,Type")] OrderImage orderImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderImage.OrderId);
            return View(orderImage);
        }

        // GET: OrderImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderImages == null)
            {
                return NotFound();
            }

            var orderImage = await _context.OrderImages.FindAsync(id);
            if (orderImage == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderImage.OrderId);
            return View(orderImage);
        }

        // POST: OrderImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OrderId,LinkUrl,Status,Type")] OrderImage orderImage)
        {
            if (id != orderImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderImageExists(orderImage.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderImage.OrderId);
            return View(orderImage);
        }

        // GET: OrderImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderImages == null)
            {
                return NotFound();
            }

            var orderImage = await _context.OrderImages
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderImage == null)
            {
                return NotFound();
            }

            return View(orderImage);
        }

        // POST: OrderImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderImages == null)
            {
                return Problem("Entity set 'ShopeeAffContext.OrderImages'  is null.");
            }
            var orderImage = await _context.OrderImages.FindAsync(id);
            if (orderImage != null)
            {
                _context.OrderImages.Remove(orderImage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderImageExists(int id)
        {
          return (_context.OrderImages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
