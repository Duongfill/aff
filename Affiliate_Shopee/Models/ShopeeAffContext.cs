using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Affiliate_Shopee.Models
{
    public partial class ShopeeAffContext : DbContext
    {
        public ShopeeAffContext()
        {
        }

        public ShopeeAffContext(DbContextOptions<ShopeeAffContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Bank> Banks { get; set; } = null!;
        public virtual DbSet<BankDeposit> BankDeposits { get; set; } = null!;
        public virtual DbSet<Complain> Complains { get; set; } = null!;
        public virtual DbSet<Deal> Deals { get; set; } = null!;
        public virtual DbSet<DealCategory> DealCategories { get; set; } = null!;
        public virtual DbSet<DealDeposit> DealDeposits { get; set; } = null!;
        public virtual DbSet<DealPaymentType> DealPaymentTypes { get; set; } = null!;
        public virtual DbSet<DealWithdraw> DealWithdraws { get; set; } = null!;
        public virtual DbSet<Deposit> Deposits { get; set; } = null!;
        public virtual DbSet<GroupFacebook> GroupFacebooks { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderHistory> OrderHistorys { get; set; } = null!;
        public virtual DbSet<OrderImage> OrderImages { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<PaymentType> PaymentTypes { get; set; } = null!;
        public virtual DbSet<Seller> Sellers { get; set; } = null!;
        public virtual DbSet<SellerProfile> SellerProfiles { get; set; } = null!;
        public virtual DbSet<StatusDeal> StatusDeals { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public virtual DbSet<Wallet> Wallets { get; set; } = null!;
        public virtual DbSet<Withdraw> Withdraws { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=103.153.69.217;Database=ShopeeAff;User Id=user2;Password=smo@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Discriminator)
                    .HasMaxLength(21)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FmcToken).HasMaxLength(500);

                entity.Property(e => e.IdBank)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NameBank).HasMaxLength(150);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Otp).HasMaxLength(500);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(500);
            });

            modelBuilder.Entity<BankDeposit>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.Gateway)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);
            });

            modelBuilder.Entity<Complain>(entity =>
            {
                entity.ToTable("Complain");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdat)
                    .HasColumnType("datetime")
                    .HasColumnName("createdat");

                entity.Property(e => e.Descriptions).HasColumnName("descriptions");

                entity.Property(e => e.Orderid).HasColumnName("orderid");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Userid)
                    .HasMaxLength(450)
                    .HasColumnName("userid");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Complains)
                    .HasForeignKey(d => d.Orderid)
                    .HasConstraintName("FK__Complain__orderi__30C33EC3");
            });

            modelBuilder.Entity<Deal>(entity =>
            {
                entity.Property(e => e.AffiliateLink).HasMaxLength(500);

                entity.Property(e => e.Code)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedName).HasMaxLength(500);

                entity.Property(e => e.DealExpiredAt).HasColumnType("datetime");

                entity.Property(e => e.ImageDeal).HasColumnName("Image_deal");

                entity.Property(e => e.LinkFacebook)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Location).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.RefundType)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ShopeeLink)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedName).HasMaxLength(500);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Deals)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Deals_DealCategories");

                entity.HasOne(d => d.GroupFacebook)
                    .WithMany(p => p.Deals)
                    .HasForeignKey(d => d.GroupFacebookId)
                    .HasConstraintName("FK_Deals_GroupFacebooks");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Deals)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Deals_AspNetUsers");
            });

            modelBuilder.Entity<DealCategory>(entity =>
            {
                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<DealDeposit>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.RefundPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TotalCashbackAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Deal)
                    .WithMany(p => p.DealDeposits)
                    .HasForeignKey(d => d.DealId)
                    .HasConstraintName("FK_DealDeposits_Deals");

                entity.HasOne(d => d.Transaciton)
                    .WithMany(p => p.DealDeposits)
                    .HasForeignKey(d => d.TransacitonId)
                    .HasConstraintName("FK_DealDeposits_Transactions");
            });

            modelBuilder.Entity<DealPaymentType>(entity =>
            {
                entity.HasOne(d => d.Deal)
                    .WithMany(p => p.DealPaymentTypes)
                    .HasForeignKey(d => d.DealId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DealPaymentTypes_Deals");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.DealPaymentTypes)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DealPaymentTypes_PaymentTypes");
            });

            modelBuilder.Entity<DealWithdraw>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.RefundAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RefundPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TotalCashbackAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Deal)
                    .WithMany(p => p.DealWithdraws)
                    .HasForeignKey(d => d.DealId)
                    .HasConstraintName("FK_DealWithdraws_Deals");

                entity.HasOne(d => d.Transaciton)
                    .WithMany(p => p.DealWithdraws)
                    .HasForeignKey(d => d.TransacitonId)
                    .HasConstraintName("FK_DealWithdraws_Transactions");
            });

            modelBuilder.Entity<Deposit>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.Gateway)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Deposits)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_Deposits_Transactions");
            });

            modelBuilder.Entity<GroupFacebook>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(500);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt2).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt3).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Deal)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DealId)
                    .HasConstraintName("FK_Orders_Deals");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserID");
            });

            modelBuilder.Entity<OrderHistory>(entity =>
            {
                entity.Property(e => e.Action).HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("date");

                entity.Property(e => e.Time).HasColumnType("date");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderHistories)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderHistorys_Orders");
            });

            modelBuilder.Entity<OrderImage>(entity =>
            {
                entity.Property(e => e.LinkUrl).HasMaxLength(500);

                entity.Property(e => e.LinkUrl2).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Status).HasMaxLength(10);

                entity.Property(e => e.Type).HasMaxLength(500);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderImages)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderImages_Orders");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Order_status");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Seller>(entity =>
            {
                entity.ToTable("SELLER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATED_AT");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Img)
                    .HasMaxLength(100)
                    .HasColumnName("IMG");

                entity.Property(e => e.Linkfb)
                    .HasMaxLength(100)
                    .HasColumnName("LINKFB");

                entity.Property(e => e.NameShop)
                    .HasMaxLength(100)
                    .HasColumnName("NAME_SHOP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATED_AT");

                entity.Property(e => e.Userid)
                    .HasMaxLength(450)
                    .HasColumnName("USERID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sellers)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("USERID");
            });

            modelBuilder.Entity<SellerProfile>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.Fullname).HasMaxLength(500);

                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.Property(e => e.LinkFacebook).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.Updatedby).HasMaxLength(500);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SellerProfiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_SellerProfiles_AspNetUsers");
            });

            modelBuilder.Entity<StatusDeal>(entity =>
            {
                entity.ToTable("Status_Deal");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreateBy).HasMaxLength(450);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.EndAmout).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Note).HasMaxLength(4000);

                entity.Property(e => e.StartAmout).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Transactions_AspNetUsers");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("UserProfile");

                entity.Property(e => e.Avartar).HasMaxLength(500);

                entity.Property(e => e.BankName).HasMaxLength(500);

                entity.Property(e => e.BankNumber).HasMaxLength(100);

                entity.Property(e => e.BankQrCode).HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(500);

                entity.Property(e => e.FullName).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.UpdatedBy).HasMaxLength(500);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK_UserProfile_Banks");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserProfile_AspNetUsers");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.OldAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Wallets_AspNetUsers");
            });

            modelBuilder.Entity<Withdraw>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BankName).HasMaxLength(150);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.OldBlance).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.QrCode).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Withdraws)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_Withdraws_Transactions");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
