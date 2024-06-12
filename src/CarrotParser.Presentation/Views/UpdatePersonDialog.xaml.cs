using CarrotParser.Presentation.ViewModels;
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
    /// Interaction logic for PersonDialogWindow.xaml
    /// </summary>
    public partial class UpdatePersonDialog : UserControl
    {
        public UpdatePersonDialog()
        {
            InitializeComponent();
        }

        private void OnPasswordChanged(object sender, System.EventArgs e)
        {
            if (DataContext is null)
            {
                return;
            }
            if (DataContext is not UpdatePersonDialogViewModel viewModel)
            {
                MessageBox.Show("Can't update password, wrong viewmodel.");
                return;
            }
            viewModel.Person.Login.Password = ((PasswordBox)sender).Password;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var window = Parent as Window ?? throw new NullReferenceException("No window found for DeleteConfirmationDialog");
            window.Close();
        }
    }
}
