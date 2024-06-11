using CarrotParser.Presentation.ViewModels.Common.Interfaces;
using CarrotParser.Presentation.ViewModels.Dialog;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CarrotParser.Presentation.ViewModels.Common;

internal class DialogService : IDialogService
{
    private static readonly Dictionary<Type, Type> _mappings = [];

    public static void RegisterDialog<TView, TViewModel>()
        where TView : FrameworkElement
        where TViewModel : ViewModelBase
    {
        _mappings.Add(typeof(TViewModel), typeof(TView));
    }

    public void ShowDialog<TViewModel>(Action<object> callback) where TViewModel : ViewModelBase, IDialogViewModel
    {
        var viewType = _mappings[typeof(TViewModel)];
        ViewModelBase viewModel = (ViewModelBase)App.ApplicationHost.Services.GetRequiredService(typeof(TViewModel));
        DialogWindow _dialogWindow = new();
        var content = App.ApplicationHost.Services.GetRequiredService(viewType);
        var cont = (FrameworkElement)content;
        ((IDialogViewModel)viewModel).OnResult += callback;
        cont.DataContext = viewModel;
        _dialogWindow.Content = content;
        _dialogWindow.ShowDialog();
    }
}
