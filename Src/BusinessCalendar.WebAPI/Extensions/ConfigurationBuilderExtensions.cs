namespace BusinessCalendar.WebAPI.Extensions;

public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Reads additional configuration files.
    /// appsettings.json is expected to be read by convention at earlier stages
    /// </summary>
    /// <param name="configurationBuilder"></param>
    /// <param name="environmentName"></param>
    /// <returns>see <see cref="ConfigurationManager"/></returns>
    public static IConfigurationBuilder ExtendConfiguration(this IConfigurationBuilder configurationBuilder, string environmentName)
    {
        foreach (var settings in new [] { "mongodb_settings", "postgres_settings", "logger"})
        {
            configurationBuilder.AddJsonFile($"{settings}.json", false);
            configurationBuilder.AddJsonFile($"{settings}.{environmentName}.json", true);
        }

        configurationBuilder.AddEnvironmentVariables();
        return configurationBuilder;
    }
}