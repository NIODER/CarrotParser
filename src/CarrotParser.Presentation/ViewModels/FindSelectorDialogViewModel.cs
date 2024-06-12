using CarrotParser.Presentation.Model;
using CarrotParser.Presentation.ViewModels.Common;
using CarrotParser.Presentation.ViewModels.Common.Interfaces;

namespace CarrotParser.Presentation.ViewModels;

public class FindSelectorDialogViewModel : ViewModelBase, IDialogViewModel
{
    public event Action<object>? OnResult;
    private FindSelector _findSelector = new(null, null, null, null);

    public RelayCommand SearchCommand { get; private set; }
    public RelayCommand ClearCommand { get; private set; }

    public FindSelectorDialogViewModel()
    {
        SearchCommand = new(OnSearchCommand);
        ClearCommand = new(OnClearCommand);
    }

    private void OnSearchCommand(object obj)
    {
        OnResult?.Invoke(_findSelector);
    }

    private void OnClearCommand(object obj)
    {
        FindSelector = new(null, null, null, null);
    }

    public FindSelector FindSelector
    {
        get => _findSelector;
        set
        {
            _findSelector = value;
            OnPropertyChanged(nameof(FindSelector));
        }
    }
}
