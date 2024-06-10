namespace CarrotParser.Presentation.ViewModels.Common.Interfaces;

internal interface IDialogService
{
    void ShowDialog(ViewModelBase viewModel, Action<bool> callback, string? questionText);
    void ShowDialog<TViewModel>(Action<bool> callback, string? questionText) where TViewModel : ViewModelBase, IDialogViewModel;
}
