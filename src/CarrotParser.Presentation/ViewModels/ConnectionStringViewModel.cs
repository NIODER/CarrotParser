using CarrotParser.Application;
using CarrotParser.Presentation.ViewModels.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotParser.Presentation.ViewModels
{
    public class ConnectionStringViewModel : ViewModelBase
    {
        private string _connectionString;

        public ConnectionStringViewModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>(DepedencyInjection.DB_CONFIGURATION_SECTION_NAME) ?? string.Empty;
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
