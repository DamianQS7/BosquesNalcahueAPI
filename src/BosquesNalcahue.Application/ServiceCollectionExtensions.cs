using BosquesNalcahue.Application.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BosquesNalcahue.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReportsApp(this IServiceCollection services)
    {
        services.AddSingleton<IReportsRepository, ReportsRepository>();
        services.AddSingleton<IAnalyticsRepository, AnalyticsRepository>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

        return services;
    }
}
