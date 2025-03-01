﻿using CandidateHub.Data.Contexts;
using CandidateHub.Data.Repositories;
using CandidateHub.Service.Interfaces;
using CandidateHub.Service.Services;
using Microsoft.EntityFrameworkCore;

namespace CandidateHub.Api.Extensions;

public static class ServiceExtensions
{
    public static void AddCustomContext(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext> (opt => opt.UseSqlite("Data Source = HubDb"));
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICandidateService, CandidateService>();
    }

}
