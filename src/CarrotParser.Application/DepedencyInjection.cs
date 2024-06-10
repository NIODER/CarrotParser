using CarrotParser.Application.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarrotParser.Application;

public static class DepedencyInjection
{
    public const string DB_CONFIGURATION_SECTION_NAME = "DbConfiguration";

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbManager(configuration);
        services.AddTransient(s => s.GetRequiredService<IDbManager>().Repository);
        return services;
    }

    private static IServiceCollection AddDbManager(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConfig = configuration.GetSection(DB_CONFIGURATION_SECTION_NAME);
        if (!dbConfig.Exists())
        {
            throw new ArgumentNullException(nameof(configuration), $"No \"{DB_CONFIGURATION_SECTION_NAME}\" section found in configuation.");
        }
        var dbPath = configuration.GetSection(DB_CONFIGURATION_SECTION_NAME).Value ?? string.Empty;
        services.AddSingleton<IDbManager, DbManager>(s => new DbManager(dbPath));
        return services;
    }
}
