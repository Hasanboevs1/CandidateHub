using CandidateHub.Data.Contexts;
using CandidateHub.Data.Repositories;
using CandidateHub.Service.Interfaces;
using CandidateHub.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CandidateHub.Api.Extensions;

public static class ServiceExtensions
{
    public static void AddCustomContext(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source = HubDb"));
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICandidateService, CandidateService>();
    }

    public static void AddSwaggerDoc(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Candidate Hub API",
                Version = "v1",
                Description = "API for managing job candidates' contact information.",
                Contact = new OpenApiContact
                {
                    Name = "Hasanboyev Muhammadyusuf",
                    Email = "hasanboevmuhammadyusuf@gmail.com",
                    Url = new Uri("https://github.com/Hasanboevs1")
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }
}