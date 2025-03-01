using CandidateHub.Service.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CandidateHub.Api.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
                .AddDbContextCheck<CandidateHub.Data.Contexts.AppDbContext>("Database")
                .AddCheck("Self", () => HealthCheckResult.Healthy("Healthy"))
                .AddCheck<CandidateHealthCheck>("Candidate Service");

        return services;
    }

    public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health");
        });

        return app;
    }


}

public class CandidateHealthCheck : IHealthCheck
{
    private readonly ICandidateService _candidateService;

    public CandidateHealthCheck(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var candidates = await _candidateService.GetAllCandidatessAsync();
            if (candidates != null)
            {
                return HealthCheckResult.Healthy("Candidate Service is healthy.");
            }
            return HealthCheckResult.Unhealthy("Candidate Service is unhealthy.");
        }
        catch
        {
            return HealthCheckResult.Unhealthy("Candidate Service is unhealthy.");
        }
    }
}