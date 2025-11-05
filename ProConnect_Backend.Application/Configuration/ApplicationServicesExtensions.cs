using Microsoft.Extensions.DependencyInjection;

namespace ProConnect_Backend.Application.Configuration;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        
        services.AddAutoMapper(typeof(ApplicationServicesExtensions).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ApplicationServicesExtensions).Assembly));
        
        return services;
    }
}