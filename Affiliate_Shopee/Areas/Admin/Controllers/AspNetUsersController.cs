using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Affiliate_Shopee.Models;
using X.PagedList;
using static Affiliate_Shopee.Areas.Admin.Controllers.ComplainsController;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Affiliate_Shopee.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace Affiliate_Shopee.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AspNetUsersController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ShopeeAffContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Affiliate_ShopeeUser> _userManager;
        public class CreateUserModel
        {
            [Required(ErrorMessage = "Email không được để trống")]
            [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Tên không được để trống")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Số điện thoại không được để trống")]
            [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại phải đủ 10 số và bắt đầu bằng số 0")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Mật khẩu không được để trống")]
            [StringLength(16, MinimumLength = 8, ErrorMessage = "Mật khẩu phải dài từ 8 đến 16 ký tự")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$", ErrorMessage = "Mật khẩu tối thiểu từ 8 ký tự bao gồm chữ hoa,thường,số và ký tự đặc biệt")]
            public string Password { get; set; }
        }

        public AspNetUsersController(ShopeeAffContext context, UserManager<Affiliate_ShopeeUser> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        // GET: Admin/AspNetUsers
        public async Task<IActionResult> Index(int? page, string phonenumber, string status, string discriminator)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var userQuery = _context.AspNetUsers.AsQueryable();
            userQuery = userQuery.OrderBy(u => u.DateCreated);

            // Lọc theo số điện thoại nếu được cung cấp
            if (!string.IsNullOrEmpty(phonenumber))
            {
                userQuery = userQuery.Where(u => u.PhoneNumber == phonenumber);
            }
            if (!string.IsNullOrEmpty(status))
            {
                userQuery = userQuery.Where(u => u.Status == status);
            }
            if (!string.IsNullOrEmpty(discriminator))
            {
                userQuery = userQuery.Where(u => u.Discriminator == discriminator);
            }
            var users = await userQuery.ToPagedListAsync(pageNumber, pageSize);
            ViewBag.Phonenumber = phonenumber;
            ViewBag.Status = status;
            ViewBag.Discriminator = discriminator;
            return View(users);
        }

        [HttpPost]
        public IActionResult UpdateStatus(string id, string status)
        {
            var item = _context.AspNetUsers.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                string statusReasonValue = status;
                item.Status = statusReasonValue;    
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.AspNetUsers == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        // GET: Admin/AspNetUsers/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Affiliate_ShopeeUser
                {
                    UserName = model.PhoneNumber,
                    Name = model.Name,
                    Email = model.Email,
                    Discriminator = "Nhân viên hỗ trợ",
                    Status = "Đang hoạt động",
                    EmailConfirmed = true,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: Admin/AspNetUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.AspNetUsers == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            return View(aspNetUser);
        }

        // POST: Admin/AspNetUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,Discriminator,Name,DateCreated,NameBank,IdBank")] AspNetUser aspNetUser)
        {
            if (id != aspNetUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserExists(aspNetUser.Id))
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
            return View(aspNetUser);
        }

        // GET: Admin/AspNetUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AspNetUsers == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        // POST: Admin/AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AspNetUsers == null)
            {
                return Problem("Entity set 'ShopeeAffContext.AspNetUsers'  is null.");
            }
            var aspNetUser = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUser != null)
            {
                _context.AspNetUsers.Remove(aspNetUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUserExists(string id)
        {
          return (_context.AspNetUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
