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
        return services;
    }

    private static IServiceCollection AddDbManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbManager, DbManager>();
        return services;
    }
}
