using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProductApi.Infrastructure.DependencyInjection;

using Application.Interfaces;
using Data;
using Repositories;
public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        // Add database connectiontivity
        // Add authentication schema
        var fileName = configuration["MySeriLog:FileName"];
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException(nameof(fileName), "Serilog file name cannot be null or empty.");
        }

        SharedServiceContainer.AddSharedServices<ProductDbContext>(services, configuration, fileName);

        // Create Dependency Injection (DI)
        services.AddScoped<IProduct, ProductRepository>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
    {
        // Register middleware such as:
        // Global exception handler external errors
        // Listion to Only Api Gateway: blocks all outsider calls
        SharedServiceContainer.UseSharedPolicies(app);

        return app;
    }
}
