using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Affiliate_Shopee.Models;

namespace Affiliate_Shopee.Services
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ShopeeAffContext _dbContext;

        public MyBackgroundService(ShopeeAffContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               
                var expiredDeals = await _dbContext.Deals
                    .Where(d => d.DealExpired == 0 && d.DealExpiredAt <= DateTime.Now)
                    .ToListAsync();

                foreach (var deal in expiredDeals)
                {
                    deal.Status = "Hết hạn";
                }
                await _dbContext.SaveChangesAsync();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
