using System.Windows.Input;

namespace CarrotParser.Presentation.ViewModels.Common;

public class RelayCommand(Action<object> methodToExecute, Func<bool>? canExecuteEvaulator) : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    private readonly Action<object> methodToExecute = methodToExecute;
    private readonly Func<bool>? canExecuteEvaulator = canExecuteEvaulator;

    public RelayCommand(Action<object> methodToExecute) : this(methodToExecute, null)
    {
    }

    public bool CanExecute(object? parameter)
    {
        if (canExecuteEvaulator is null)
        {
            return true;
        }
        else
        {
            return canExecuteEvaulator.Invoke();
        }
    }

    public void Execute(object? parameter)
    {
        methodToExecute.Invoke(parameter ?? new());
    }
}
