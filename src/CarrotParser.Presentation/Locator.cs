using CarrotParser.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CarrotParser.Presentation;

internal class Locator
{
    public MainViewModel MainViewModel => App.ApplicationHost.Services.GetRequiredService<MainViewModel>();
    public ConnectionStringViewModel ConnectionStringViewModel => App.ApplicationHost.Services.GetRequiredService<ConnectionStringViewModel>();
}
