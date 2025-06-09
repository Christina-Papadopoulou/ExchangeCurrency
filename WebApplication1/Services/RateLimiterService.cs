using Microsoft.Extensions.Caching.Memory;

namespace WalletAppication.Services
{
    public class RateLimiterService
    {
        private readonly IMemoryCache _cache;
        private readonly int _maxRequestsPerMinute;
        private readonly TimeSpan _timeWindow;

        public RateLimiterService(IMemoryCache cache, int maxRequestsPerMinute = 100, TimeSpan? timeWindow = null)
        {
            _cache = cache;
            _maxRequestsPerMinute = maxRequestsPerMinute;
            _timeWindow = timeWindow ?? TimeSpan.FromMinutes(1);  // Default time window of 1 minute
        }

        public bool IsRateLimited(string ipAddress, string endpoint)
        {
            var cacheKey = $"rate_limit_{ipAddress}-{endpoint}";
            var currentRequests = _cache.Get<int>(cacheKey);

            // If the number of requests exceeds the limit, the client is rate-limited
            if (currentRequests >= _maxRequestsPerMinute)
            {
                return true;
            }

            // Otherwise, increment the request count and set the expiration time
            _cache.Set(cacheKey, currentRequests + 1, _timeWindow);
            return false;
        }
    }
}
