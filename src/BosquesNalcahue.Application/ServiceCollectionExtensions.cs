using BosquesNalcahue.Application.Models;
using BosquesNalcahue.Application.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BosquesNalcahue.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReportsApp(this IServiceCollection services)
    {
        services.AddSingleton<IReportsRepository, ReportsRepository>();
        return services;
    }
}
