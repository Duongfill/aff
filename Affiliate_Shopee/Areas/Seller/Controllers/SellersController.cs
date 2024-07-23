using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Affiliate_Shopee.Models;
using Microsoft.AspNetCore.Authorization;

namespace Affiliate_Shopee.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class SellersController : Controller
    {
        private readonly ShopeeAffContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SellersController(ShopeeAffContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Seller/Sellers
        public async Task<IActionResult> Index()
        {
            
            var shopeeAffContext = _context.Sellers.Include(s => s.User);
            return View(await shopeeAffContext.ToListAsync());
        }

        // GET: Seller/Sellers/Details/5
        public async Task<IActionResult> Details()
        {
            var id = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            if (id == null || _context.Sellers == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // GET: Seller/Sellers/Create
        public IActionResult Create()
        {
            ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,NameShop,Email,Linkfb,Img,Userid,CreatedAt,UpdatedAt")] Seller seller)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(seller);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id", seller.Userid);
        //    return View(seller);
        //}

        //// GET: Seller/Sellers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Sellers == null)
        //    {
        //        return NotFound();
        //    }

        //    var seller = await _context.Sellers.FindAsync(id);
        //    if (seller == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id", seller.Userid);
        //    return View(seller);
        //}

        //// POST: Seller/Sellers/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,NameShop,Email,Linkfb,Img,Userid,CreatedAt,UpdatedAt")] Seller seller)
        //{
        //    if (id != seller.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(seller);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SellerExists(seller.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id", seller.Userid);
        //    return View(seller);
        //}

        // GET: Seller/Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sellers == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // POST: Seller/Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sellers == null)
            {
                return Problem("Entity set 'ShopeeAffContext.Sellers'  is null.");
            }
            var seller = await _context.Sellers.FindAsync(id);
            if (seller != null)
            {
                _context.Sellers.Remove(seller);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerExists(int id)
        {
          return (_context.Sellers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
