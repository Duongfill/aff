using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            Deals = new HashSet<Deal>();
            Orders = new HashSet<Order>();
            SellerProfiles = new HashSet<SellerProfile>();
            Sellers = new HashSet<Seller>();
            Transactions = new HashSet<Transaction>();
            UserProfiles = new HashSet<UserProfile>();
            Wallets = new HashSet<Wallet>();
            Roles = new HashSet<AspNetRole>();
        }

        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Discriminator { get; set; } = null!;
        public string? Name { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? NameBank { get; set; }
        public string? IdBank { get; set; }
        public string? Status { get; set; }
        public string? Otp { get; set; }
        public string? FmcToken { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<SellerProfile> SellerProfiles { get; set; }
        public virtual ICollection<Seller> Sellers { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
