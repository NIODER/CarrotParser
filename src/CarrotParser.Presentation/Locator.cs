using CarrotParser.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CarrotParser.Presentation;

public class Locator
{
    public MainViewModel MainViewModel => App.ApplicationHost.Services.GetRequiredService<MainViewModel>();
    public ConnectionStringDialogViewModel ConnectionStringViewModel => App.ApplicationHost.Services.GetRequiredService<ConnectionStringDialogViewModel>();
    public UpdatePersonDialogViewModel UpdatePersonDialogViewModel => App.ApplicationHost.Services.GetRequiredService<UpdatePersonDialogViewModel>();
}
