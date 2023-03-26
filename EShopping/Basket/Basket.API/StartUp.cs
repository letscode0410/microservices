using System.Reflection;
using Basket.Application.Queries;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Basket.API;

public class StartUp
{
    private readonly IConfiguration _configuration;

    public StartUp(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();
        //Redis 
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = _configuration.GetValue<string>("CacheSettings:ConnectionString");
        });

        services.AddHealthChecks().AddRedis(_configuration.GetValue<string>("CacheSettings:ConnectionString"),
            "Redis Health", HealthStatus.Degraded);
        
        services.AddSwaggerGen(s => s.SwaggerDoc("v1", new OpenApiInfo() { Title = "Basket.API", Version = "V1" }));
        services.AddAutoMapper(typeof(StartUp));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetBasketByUserNameQuery).GetTypeInfo().Assembly));
        services.AddScoped<IBasketRepository, BasketRepository>();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API"));
        }

        app.UseRouting();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _=>true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}