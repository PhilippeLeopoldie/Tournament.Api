using Microsoft.EntityFrameworkCore;
using Tournament.Infrastructure.Data;

namespace Tournament.Api.Extensions;

public static class ServiceExtensions
{

    
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLazy<TService>(this IServiceCollection services) where TService : class
    {
        return services.AddScoped(provider => new Lazy<TService>(() => provider.GetRequiredService<TService>()));
    }
}
