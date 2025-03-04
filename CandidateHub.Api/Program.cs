using CandidateHub.Api.Extensions;
using CandidateHub.Api.Middlewares;
using CandidateHub.Data.Contexts;
using CandidateHub.Service.Mappers;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomContext();

builder.Services.AddCustomServices();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddSwaggerDoc();
builder.Services.AddMemoryCache();
builder.Services.AddCustomHealthChecks();
builder.Services.CustomRateLimiterService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlerMiddleWare>();
app.UseMiddleware<RateLimitingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseCustomHealthChecks();

app.Run();
