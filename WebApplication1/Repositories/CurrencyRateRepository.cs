using ECBGateway.Model;
using Microsoft.EntityFrameworkCore;
using WalletApplication.Domain;
using WalletApplication.Entities;

namespace WalletAppication.Repositories
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        private readonly AppDbContext _dbContext;

        public CurrencyRateRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ECBRate>> GetAllCurrencies()
        {
            return await _dbContext.CurrencyRates.ToListAsync();
        }

       
    }
}
