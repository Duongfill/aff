using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Affiliate_Shopee.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Affiliate_Shopee.Areas.Identity.Data;

namespace Affiliate_Shopee.Controllers
{
    public class SellersController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ShopeeAffContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Affiliate_ShopeeUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Affiliate_ShopeeUser> _signInManager;

        public  SellersController(ShopeeAffContext context,
                UserManager<Affiliate_ShopeeUser> userManager,
                RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor, 
            IWebHostEnvironment hostingEnvironment, 
            SignInManager<Affiliate_ShopeeUser> signInManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var shopeeAffContext = _context.Sellers.Include(s => s.User);
            return View(await shopeeAffContext.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
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

        // GET: Sellers/Create
        public IActionResult Create()
        {
            ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string NameShop, string Linkfb, string Nameimg, IFormFile ImgFB)
        {
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "image/imagefacebook");
            var uniqueFileName = Nameimg;

            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            var checkuserid = await _context.Sellers.FirstOrDefaultAsync(s => s.Userid == userId);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            if (string.IsNullOrEmpty(NameShop))
            {
                return Json(new { success = false, message = "Tên shop không được để trống." });
            }
            if (string.IsNullOrEmpty(Linkfb))
            {
                return Json(new { success = false, message = "Link Facebook không được để trống." });
            }
            if (string.IsNullOrEmpty(Nameimg))
            {
                return Json(new { success = false, message = "Hình ảnh không được để trống." });
            }
            if (checkuserid != null)
            {
                return Json(new { success = false, message = "Người dùng đã tồn tại." });
            }
            else
            {
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                if (userId == null)
                {
                    return Json(new { success = false, message = "Người dùng không hợp lệ." });
                }
                var existingSeller = await _context.Sellers.FirstOrDefaultAsync(s => s.Linkfb == Linkfb);
                if (existingSeller != null)
                {
                    return Json(new { success = false, message = "Facebook đã được sử dụng." });
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImgFB.CopyToAsync(stream);
                }
                var createdat = DateTime.Now;
                var seller = new Seller
                {
                    Userid = userId,
                    NameShop = NameShop,
                    Linkfb = Linkfb,
                    CreatedAt = createdat,
                    Img = Nameimg
                };
                _context.Sellers.Add(seller);
                await _context.SaveChangesAsync();
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    // Xóa quyền cũ
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);

                    // Thêm quyền mới
                    var newRole = "Seller"; // Tên của quyền bạn muốn thêm
                    if (await _roleManager.RoleExistsAsync(newRole))
                    {
                        await _userManager.AddToRoleAsync(user, newRole);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Bạn không được cấp quyền" });
                    }
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                return Json(new { success = true, redirectUrl = Url.Action("Details", "Sellers", new { area = "Seller" }) });

            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sellers == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id", seller.Userid);
            return View(seller);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameShop,Email,Linkfb,Img,Userid")] Seller seller)
        {
            if (id != seller.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(seller.Id))
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
            ViewData["Userid"] = new SelectList(_context.AspNetUsers, "Id", "Id", seller.Userid);
            return View(seller);
        }

        // GET: Sellers/Delete/5
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

        // POST: Sellers/Delete/5
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
