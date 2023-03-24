using System.Reflection;
using System.Runtime.Intrinsics;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repository;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Catalog.API;

public class StartUp
{
    private readonly IConfiguration _configuration;

    public StartUp(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();
        services.AddHealthChecks().AddMongoDb(_configuration["DatabaseSettings:ConnectionString"],
            "Catalog Mongo DB Health Check", HealthStatus.Degraded);
        services.AddSwaggerGen(s =>
            s.SwaggerDoc("v1", new OpenApiInfo() { Title = "Catalog.API", Version = "v1" }));

        services.AddAutoMapper(typeof(StartUp));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllBrandsQuery).GetTypeInfo().Assembly));
        services.AddScoped<ICatalogContext, CatalogContext>();
        services.AddScoped<ITypeRepository, ProductRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBrandRepository, ProductRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}