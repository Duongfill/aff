using System;
using System.Collections.Generic;

namespace Affiliate_Shopee.Models
{
    public partial class Transaction
    {
        public Transaction()
        {
            DealDeposits = new HashSet<DealDeposit>();
            DealWithdraws = new HashSet<DealWithdraw>();
            Deposits = new HashSet<Deposit>();
            Withdraws = new HashSet<Withdraw>();
        }

        public int Id { get; set; }
        public decimal? StartAmout { get; set; }
        public decimal? EndAmout { get; set; }
        public decimal? Amount { get; set; }
        public string? UserId { get; set; }
        public byte? TransactionType { get; set; }
        public string? Note { get; set; }
        public byte? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual AspNetUser? User { get; set; }
        public virtual ICollection<DealDeposit> DealDeposits { get; set; }
        public virtual ICollection<DealWithdraw> DealWithdraws { get; set; }
        public virtual ICollection<Deposit> Deposits { get; set; }
        public virtual ICollection<Withdraw> Withdraws { get; set; }
    }
}
