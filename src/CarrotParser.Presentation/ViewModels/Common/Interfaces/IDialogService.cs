namespace CarrotParser.Presentation.ViewModels.Common.Interfaces;

public interface IDialogService
{
    void ShowDialog<TViewModel>(Action<object> callback) where TViewModel : ViewModelBase, IDialogViewModel;
}
