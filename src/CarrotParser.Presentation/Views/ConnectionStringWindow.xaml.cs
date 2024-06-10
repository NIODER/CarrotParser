using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarrotParser.Presentation.Views
{
    /// <summary>
    /// Interaction logic for ConnectionStringWindow.xaml
    /// </summary>
    public partial class ConnectionStringWindow : Window
    {
        public ConnectionStringWindow()
        {
            InitializeComponent();
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
