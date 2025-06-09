using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WalletApplication.Domain;

namespace WalletAppication.Services
{
    public class CurrencyCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly AppDbContext _dbContext;
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

        public CurrencyCacheService(IMemoryCache memoryCache, AppDbContext dbContext)
        {
            _memoryCache = memoryCache;
            _dbContext = dbContext;
        }

        // Retrieve rate from cache, or fall back to the database if not in the cache
        public async Task<decimal> GetCurrencyConversionRateAsync(string currency)
        {
            // First check if the rate is cached
            if (_memoryCache.TryGetValue(currency, out decimal rate))
            {
                return rate; // Return cached value
            }

            // If not cached, retrieve from database
            rate = await GetCurrencyConversionRateFromDatabaseAsync(currency);

            // Cache the retrieved rate for future use
            SetCurrencyRateInCache(currency, rate);

            return rate;
        }

        // Retrieves currency rate from the database
        private async Task<decimal> GetCurrencyConversionRateFromDatabaseAsync(string currency)
        {
            var currencyRate = await _dbContext.CurrencyRates
                                               .Where(w => w.Currency == currency)
                                               .Select(w => w.Rate)
                                               .FirstOrDefaultAsync();

            return currencyRate == default ? 1 : currencyRate; // Return 1 if not found
        }

        // Caches the rate with an expiration time
        private void SetCurrencyRateInCache(string currency, decimal rate)
        {
            _memoryCache.Set(currency, rate, CacheExpiration);
        }

        // Refresh the cache for all rates (optional: triggered by periodic updates)
        public void RefreshAllRates()
        {
            // Example of updating multiple rates in the cache (e.g., update all at once after periodic updates)
            var allRates = _dbContext.CurrencyRates.ToList();
            foreach (var rate in allRates)
            {
                _memoryCache.Set(rate.Currency, rate.Rate, CacheExpiration);
            }
        }
    }
}
