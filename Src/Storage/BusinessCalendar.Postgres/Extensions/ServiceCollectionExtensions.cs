using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Postgres.Options;
using BusinessCalendar.Postgres.Repositories;
using BusinessCalendar.SqlMigrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace BusinessCalendar.Postgres.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds storage services to ServiceCollection and registers NpgsqlDataSource
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddPostgresStorage(this IServiceCollection services)
    {
        services.AddSingleton<NpgsqlDataSource>(sp =>
        {
            var postgresOptions = sp.GetRequiredService<IOptions<PostgresOptions>>().Value;
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(postgresOptions.ConnectionString);
            dataSourceBuilder.UseLoggerFactory(sp.GetService<ILoggerFactory>());
            return dataSourceBuilder.Build();
        });
        
        services.AddSingleton<ICalendarIdentifierStorageService, PgCalendarIdentifierRepository>();
        services.AddSingleton<ICalendarStorageService, PgCalendarRepository>();

        services.AddFluentMigratorCore()
            .ConfigureRunner(builder =>
            {
                builder.AddPostgres()
                    .WithGlobalConnectionString(x => 
                        x.GetRequiredService<IOptions<PostgresOptions>>().Value.ConnectionString)
                    .ScanIn(typeof(InitialMigration).Assembly).For.Migrations();
            });

        return services;
    }
    
    /// <summary>
    /// Add a health check for postgres database
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/></param>
    /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'postgres' will be used for the name.</param>
    /// <param name="failureStatus">
    /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
    /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
    /// </param>
    /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
    /// <param name="timeout">An optional <see cref="TimeSpan"/> representing the timeout of the check.</param>
    public static void AddPostgresHealthCheck(
        this IServiceCollection services,
        string name = "postgres",
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        services.AddHealthChecks()
            .AddNpgSql(sp => sp.GetRequiredService<IOptions<PostgresOptions>>().Value.ConnectionString,
                name: name,
                failureStatus: failureStatus,
                tags: tags,
                timeout: timeout);
    }
}