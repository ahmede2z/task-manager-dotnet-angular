using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Api.Extensions;

/// <summary>
/// Dependency injection extensions for API services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds API-specific services to the service collection.
    /// </summary>
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        services.AddCors(options =>
        {
            options.AddPolicy("Development", builder =>
                builder
                    .WithOrigins("http://localhost:4201", "http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
        });

        services.AddHealthChecks();

        services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

        return services;
    }
}
