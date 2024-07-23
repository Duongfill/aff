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
    public class OrdersController : Controller
    {
        private readonly ShopeeAffContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersController(ShopeeAffContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index(int? page, string status)
        {
            var pagenumber = page ?? 1;
            var pageSize = 20;
            string loginUrl = Url.Action("Login", "Account", new { area = "Identity" });
            IQueryable<Order> query = _context.Orders.Include(o => o.Deal);
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            if (userId == null)
            {
                return Redirect(loginUrl);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(o => o.UserId == userId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status == status);
            }
            var orders = await query.ToPagedListAsync(pagenumber, pageSize);
            ViewBag.Status = status;
            return View(orders);
        }
        public IActionResult Create()
        {
            ViewData["DealId"] = new SelectList(_context.Deals, "Id", "Id");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DealId,Status,CreatedAt,UpdatedAt,UserId,PaymentTypeId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DealId"] = new SelectList(_context.Deals, "Id", "Id", order.DealId);
            return View(order);
        }
        public IActionResult Details(int id)
        {
            var order= _context.Deals.FirstOrDefault(p => p.Id == id);
            if (order == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(order);
        }
        public IActionResult CreateOrder()
        {
            ViewData["DealId"] = new SelectList(_context.Deals, "Id", "Id");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DealId,Status,CreatedAt,UpdatedAt,UserId,PaymentTypeId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["DealId"] = new SelectList(_context.Deals, "Id", "Id", order.DealId);
            return View(order);
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

        // POST: Orders/Delete/5
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
