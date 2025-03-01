using System.Threading.RateLimiting;

namespace CandidateHub.Api.Extensions;

public static class RateLimitingExtensions
{
    public static void CustomRateLimiterService(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string?>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,  // Allow only 5 requests
                        Window = TimeSpan.FromSeconds(10) // Per 10 seconds
                    }));

            options.RejectionStatusCode = 429; // Too Many Requests
        });
    }
}
