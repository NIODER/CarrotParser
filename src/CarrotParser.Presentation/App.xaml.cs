using CarrotParser.Application;
using CarrotParser.Presentation.ViewModels;
using CarrotParser.Presentation.ViewModels.Common;
using CarrotParser.Presentation.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        MainWindow = ApplicationHost.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();
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
        DialogService.RegisterDialog<ConnectionStringDialog, ConnectionStringDialogViewModel>();
        DialogService.RegisterDialog<UpdatePersonDialog, UpdatePersonDialogViewModel>();
        DialogService.RegisterDialog<MoveDatabaseDialog, MoveDatabaseDialogViewModel>();
    }

    private static void RegisterWindowServices()
    {
        //WindowService.RegisterWindow<ConnectionStringDialogViewModel, ConnectionStringDialogWindow>();
    }

    private static IConfiguration GetConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();
        configuration[DepedencyInjection.DB_CONFIGURATION_SECTION_NAME] = "asdf";
        return configuration;
    }
}

