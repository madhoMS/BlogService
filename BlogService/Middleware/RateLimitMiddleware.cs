using Microsoft.Extensions.Caching.Memory;

namespace BlogService.Middleware
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;

        public RateLimitMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection?.RemoteIpAddress?.ToString();
            var cacheKey = $"rate_limit_{ipAddress}";

            if (!_cache.TryGetValue(cacheKey, out int requestCount))
            {
                requestCount = 0;
            }

            requestCount++;

            if (requestCount > 10) // Adjust the limit as needed (e.g., 10 requests per minute)
            {
                context.Response.StatusCode = 429; // Rate limit exceeded
                await context.Response.WriteAsync("Rate limit exceeded.");
                return;
            }

            _cache.Set(cacheKey, requestCount, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // Reset rate limit every minute
            });

            await _next.Invoke(context);
        }
    }
}
