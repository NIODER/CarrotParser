using CarrotParser.Presentation.ViewModels.Common;
using CarrotParser.Presentation.ViewModels.Common.Interfaces;
using System.Windows;

namespace CarrotParser.Presentation.ViewModels
{
    public class MoveDatabaseDialogViewModel : ViewModelBase, IDialogViewModel
    {
        public event Action<object>? OnResult;
        private string _location = string.Empty;

        public RelayCommand MoveDatabaseCommand { get; private set; }

        public MoveDatabaseDialogViewModel()
        {
            MoveDatabaseCommand = new(OnMoveDatabaseCommand);
        }

        private void OnMoveDatabaseCommand(object obj)
        {
            if (string.IsNullOrEmpty(_location))
            {
                MessageBox.Show("Invalid location.");
                return;
            }
            OnResult?.Invoke(Location);
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }
    }
}
