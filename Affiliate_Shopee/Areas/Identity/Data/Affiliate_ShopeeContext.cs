using Affiliate_Shopee.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Affiliate_Shopee.Data;

public class Affiliate_ShopeeContext : IdentityDbContext<Affiliate_ShopeeUser>
{
    public Affiliate_ShopeeContext(DbContextOptions<Affiliate_ShopeeContext> options)
        : base(options)
    {
    }
    //public DbSet<Affiliate_ShopeeUser> Affiliate_ShopeeUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        
    }
}
