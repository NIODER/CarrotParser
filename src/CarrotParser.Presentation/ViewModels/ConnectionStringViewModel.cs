using CarrotParser.Application;
using CarrotParser.Presentation.ViewModels.Common;
using Microsoft.Extensions.Configuration;
using System.Windows;

namespace CarrotParser.Presentation.ViewModels
{
    public class ConnectionStringViewModel : ViewModelBase
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;

        public RelayCommand UpdateConnectionStringCommand { get; private set; }

        public ConnectionStringViewModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>(DepedencyInjection.DB_CONFIGURATION_SECTION_NAME) ?? string.Empty;
            UpdateConnectionStringCommand = new(OnUpdateConnectionStringCommandClick);
        }

        private void OnUpdateConnectionStringCommandClick(object obj)
        {
            _configuration.GetSection(DepedencyInjection.DB_CONFIGURATION_SECTION_NAME).Value = _connectionString;
            MessageBox.Show($"Connection string saved as \"{_connectionString}\"");
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
