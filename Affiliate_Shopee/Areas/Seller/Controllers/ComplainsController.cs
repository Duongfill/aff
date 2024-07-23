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

namespace Affiliate_Shopee.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class ComplainsController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ShopeeAffContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ComplainsController(ShopeeAffContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Seller/Complains
        public class ComplaintViewModel
        {
            public int Id { get; set; }
            public int OrderId { get; set; }
            public string DealName { get; set; }
            public string CustomerName { get; set; }
            public DateTime? CreatedAt { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public string PhoneNumber { get; set; }
            public string StatusOrder { get; set; }
        }
        // GET: Admin/Complains
        public async Task<IActionResult> Index(int? page, string status, string nameuser, DateTime? createdat)
        {
            var pageNumber = page ?? 1;
            var pageSize = 30;
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var complaintsQuery = _context.Complains
                .Where(c => c.Order.UserId == userId )
                .Include(c => c.Order)
                    .ThenInclude(o => o.User)
                .Include(c => c.Order)
                    .ThenInclude(o => o.Deal)
                .OrderByDescending(c => c.Createdat)
                .Select(c => new ComplaintViewModel
                {
                    Id = c.Id,
                    OrderId = c.Orderid.GetValueOrDefault(),
                    DealName = c.Order.Deal.Name,
                    CustomerName = c.Order.User.UserName,
                    CreatedAt = c.Createdat,
                    Description = c.Descriptions,
                    Status = c.Status,
                    StatusOrder = c.Order.Status,
                });
            //complaintsQuery = complaintsQuery.Where(c => c.StatusOrder != "Đã nhận tiền");
            if (!string.IsNullOrEmpty(status))
            {
                complaintsQuery = complaintsQuery.Where(c => c.Status == status);
            }
            if (!string.IsNullOrEmpty(nameuser))
            {
                complaintsQuery = complaintsQuery.Where(c => c.CustomerName.Contains(nameuser));
            }
            if (createdat.HasValue)
            {
                complaintsQuery = complaintsQuery.Where(c => c.CreatedAt.Value.Date == createdat.Value.Date);
            }
            var complaints = await complaintsQuery.ToPagedListAsync(pageNumber, pageSize);
            return View(complaints);
        }

        // GET: Seller/Complains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Complains == null)
            {
                return NotFound();
            }

            var complain = await _context.Complains
                .Include(c => c.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (complain == null)
            {
                return NotFound();
            }

            return View(complain);
        }

        // GET: Seller/Complains/Create
        public IActionResult Create()
        {
            ViewData["Orderid"] = new SelectList(_context.Orders, "Id", "Id");
            return View();
        }

        // POST: Seller/Complains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Userid,Orderid,Createdat,Descriptions,Status")] Complain complain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(complain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Orderid"] = new SelectList(_context.Orders, "Id", "Id", complain.Orderid);
            return View(complain);
        }

        // GET: Seller/Complains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Complains == null)
            {
                return NotFound();
            }

            var complain = await _context.Complains.FindAsync(id);
            if (complain == null)
            {
                return NotFound();
            }
            ViewData["Orderid"] = new SelectList(_context.Orders, "Id", "Id", complain.Orderid);
            return View(complain);
        }

        // POST: Seller/Complains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Userid,Orderid,Createdat,Descriptions,Status")] Complain complain)
        {
            if (id != complain.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(complain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComplainExists(complain.Id))
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
            ViewData["Orderid"] = new SelectList(_context.Orders, "Id", "Id", complain.Orderid);
            return View(complain);
        }

        // GET: Seller/Complains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Complains == null)
            {
                return NotFound();
            }

            var complain = await _context.Complains
                .Include(c => c.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (complain == null)
            {
                return NotFound();
            }

            return View(complain);
        }

        // POST: Seller/Complains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Complains == null)
            {
                return Problem("Entity set 'ShopeeAffContext.Complains'  is null.");
            }
            var complain = await _context.Complains.FindAsync(id);
            if (complain != null)
            {
                _context.Complains.Remove(complain);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComplainExists(int id)
        {
          return (_context.Complains?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
