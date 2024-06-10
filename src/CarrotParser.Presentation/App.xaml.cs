using CarrotParser.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace CarrotParser.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    public static readonly IHost ApplicationHost = GetDependencyInjection();

    protected override void OnStartup(StartupEventArgs e)
    {
        RegisterDialogServices();
        RegisterWindowServices();
        ApplicationHost.Start();
        base.OnStartup(e);
    }

    private static IHost GetDependencyInjection()
    {
        return Host.CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            var configuration = GetConfiguration();
            services.AddApplication(configuration);
            services.AddPresentation();
        }).Build();
    }

    private static void RegisterDialogServices()
    {
        //DialogService.RegisterDialog<Dialog, DialogViewModel>();
    }

    private static void RegisterWindowServices()
    {
        //WindowService.RegisterWindow<WindowViewModel, Window>();
    }

    private static IConfiguration GetConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();
        configuration["DbConfiguration"] = "";
        return configuration;
    }
}

