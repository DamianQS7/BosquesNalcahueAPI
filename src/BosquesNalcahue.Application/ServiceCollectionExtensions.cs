using BosquesNalcahue.Application.Models;
using BosquesNalcahue.Application.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BosquesNalcahue.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IReportsRepository, ReportsRepository>();
        return services;
    }

    public static IServiceCollection ConfigureMongoDb(this IServiceCollection services, IConfiguration config, string sectionKey)
    {
        services.Configure<MongoDbOptions>(optionsAction =>
        {
            config.GetSection(sectionKey);
        });

        services.AddSingleton<IMongoDbOptions>
                            (sp => sp.GetRequiredService<IOptions<MongoDbOptions>>().Value);

        return services;
    }
}
