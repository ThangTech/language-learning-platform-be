using Microsoft.Extensions.DependencyInjection;
using Shared.Common.Repositories;

namespace Shared.Common.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to register shared services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register the generic repository with DI.
    /// </summary>
    public static IServiceCollection AddGenericRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        return services;
    }
}
