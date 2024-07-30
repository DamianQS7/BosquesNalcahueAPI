﻿using Azure.Storage.Blobs;
using BosquesNalcahue.Application.Repositories;
using BosquesNalcahue.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BosquesNalcahue.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReportsApp(this IServiceCollection services)
    {
        services.AddSingleton<IReportsRepository, ReportsRepository>();
        services.AddSingleton<IAnalyticsRepository, AnalyticsRepository>();
        services.AddScoped<IdentityService>();
        services.AddScoped<JwtService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
