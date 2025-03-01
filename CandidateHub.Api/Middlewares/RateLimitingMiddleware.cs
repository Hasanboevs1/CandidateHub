namespace CandidateHub.Api.Middlewares;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly int _limit;
    private readonly TimeSpan _window;

    public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
        _limit = 5; // Maximum 5 requests
        _window = TimeSpan.FromSeconds(10); // Per 10 seconds
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(ipAddress))
        {
            await _next(context);
            return;
        }

        var cacheKey = $"RateLimit_{ipAddress}";
        var requestCount = _cache.Get<int>(cacheKey);

        if (requestCount >= _limit)
        {
            context.Response.StatusCode = 429;
            await context.Response.WriteAsync("Too many requests. Try again later.");
            return;
        }

        _cache.Set(cacheKey, requestCount + 1, _window);
        await _next(context);
    }
}
