namespace CarrotParser.Presentation.ViewModels.Common.Interfaces;

internal interface IWindowService
{
    public IViewModel Show<IViewModel>() where IViewModel : ViewModelBase;
    public void Show(ViewModelBase viewModel);
}
