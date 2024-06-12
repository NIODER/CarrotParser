using CarrotParser.Application;
using CarrotParser.Presentation.ViewModels.Common;
using CarrotParser.Presentation.ViewModels.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Windows;

namespace CarrotParser.Presentation.ViewModels
{
    public class ConnectionStringDialogViewModel : ViewModelBase, IDialogViewModel
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;

        public event Action<object>? OnResult;

        public RelayCommand UpdateConnectionStringCommand { get; private set; }

        public ConnectionStringDialogViewModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>(DepedencyInjection.DB_CONFIGURATION_SECTION_NAME) ?? string.Empty;
            UpdateConnectionStringCommand = new(OnUpdateConnectionStringCommandClick);
        }

        private void OnUpdateConnectionStringCommandClick(object obj)
        {
            _configuration.GetSection(DepedencyInjection.DB_CONFIGURATION_SECTION_NAME).Value = _connectionString;
            OnResult?.Invoke(ConnectionString);
        }

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                OnPropertyChanged(nameof(ConnectionString));
            }
        }
    }
}
