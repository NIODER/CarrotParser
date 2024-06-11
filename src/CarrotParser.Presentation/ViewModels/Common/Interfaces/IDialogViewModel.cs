namespace CarrotParser.Presentation.ViewModels.Common.Interfaces;

public interface IDialogViewModel
{
    event Action<object>? OnResult;
}