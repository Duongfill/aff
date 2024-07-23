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
        }
        // GET: Admin/Complains
        public async Task<IActionResult> Index(int? page, string status, string nameuser, DateTime? createdat)
        {
            var pageNumber = page ?? 1;
            var pageSize = 30;
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var complaintsQuery = _context.Complains
                
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
                    PhoneNumber = c.Order.User.PhoneNumber
                });
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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var complain = await _context.Complains
                    .Include(c => c.Order)
                    .ThenInclude(o => o.OrderImages) 
                    .FirstOrDefaultAsync(m => m.Id == id);
            if (complain == null)
            {
                return NotFound();
            }
            return View(complain);
        }

        // GET: Admin/Complains/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Complains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Userid,Nameuser,Orderid,Createdat,Descriptions")] Complain complain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(complain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(complain);
        }

        // GET: Admin/Complains/Edit/5
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
            return View(complain);
        }

        // POST: Admin/Complains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string status)
        {
            var complain = await _context.Complains.FindAsync(id);
            if (complain == null)
            {
                return NotFound();
            }
            complain.Status = status;

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
            return View(complain);
        }

        // GET: Admin/Complains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Complains == null)
            {
                return NotFound();
            }

            var complain = await _context.Complains
                .FirstOrDefaultAsync(m => m.Id == id);
            if (complain == null)
            {
                return NotFound();
            }

            return View(complain);
        }

        // POST: Admin/Complains/Delete/5
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
