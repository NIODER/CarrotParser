using System.Windows;
using System.Windows.Controls;

namespace CarrotParser.Presentation.Views
{
    /// <summary>
    /// Interaction logic for ConnectionStringWindow.xaml
    /// </summary>
    public partial class ConnectionStringDialog : UserControl
    {
        public ConnectionStringDialog()
        {
            InitializeComponent();
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            var window = Parent as Window ?? throw new NullReferenceException();
            window.Close();
        }
    }
}
