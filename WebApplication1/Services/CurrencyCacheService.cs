using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WalletAppication.Interfaces;
using WalletAppication.Repositories;
using WalletApplication.Domain;

namespace WalletAppication.Services
{
    public class CurrencyCacheService : ICurrencyCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

        public CurrencyCacheService(IMemoryCache memoryCache, ICurrencyRateRepository currencyRateRepository)
        {
            _memoryCache = memoryCache;
            _currencyRateRepository = currencyRateRepository;
        }

        // Retrieve rate from cache, or fall back to the database if not in the cache
        public decimal GetCurrencyConversionRateAsync(string currency)
        {
            // First check if the rate is cached
            if (_memoryCache.TryGetValue(currency, out decimal rate))
            {
                return rate; // Return cached value
            }

            // If not cached, retrieve from database
            rate = GetCurrencyConversionRateFromDatabaseAsync(currency);

            // Cache the retrieved rate for future use
            SetCurrencyRateInCache(currency, rate);

            return rate;
        }

        // Retrieves currency rate from the database
        private decimal GetCurrencyConversionRateFromDatabaseAsync(string currency)
        {
            var allRates = _currencyRateRepository.GetAllCurrencies().Result;
            var currencyRate = allRates.FirstOrDefault(w => w.Currency.ToLower() == currency.ToLower());
            if (currencyRate == null)
                return 1;
            return currencyRate.Rate; 
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
            var allRates = _currencyRateRepository.GetAllCurrencies().Result;
            foreach (var rate in allRates)
            {
                _memoryCache.Set(rate.Currency, rate.Rate, CacheExpiration);
            }
        }
    }
}
