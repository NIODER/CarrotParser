using CarrotParser.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CarrotParser.Presentation;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddViews();
        services.AddSingleton<Locator>();
        services.AddViewModels();
        return services;
    }

    private static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        return services;
    }

    private static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        return services;
    }
}
