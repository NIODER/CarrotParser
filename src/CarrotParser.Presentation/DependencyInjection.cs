using CarrotParser.Presentation.ViewModels;
using CarrotParser.Presentation.ViewModels.Common;
using CarrotParser.Presentation.ViewModels.Common.Interfaces;
using CarrotParser.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CarrotParser.Presentation;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSingleton<Locator>();
        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddViews();
        services.AddViewModels();
        return services;
    }

    private static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddTransient<ConnectionStringWindow>();
        return services;
    }

    private static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddTransient<ConnectionStringViewModel>();
        return services;
    }
}
