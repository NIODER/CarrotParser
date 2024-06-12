using System.Windows;
using System.Windows.Controls;

namespace CarrotParser.Presentation.Views
{
    /// <summary>
    /// Interaction logic for FindSelectorDialog.xaml
    /// </summary>
    public partial class FindSelectorDialog : UserControl
    {
        public FindSelectorDialog()
        {
            InitializeComponent();
        }

        private void OnSearchCommandClick(object sender, RoutedEventArgs e)
        {
            var window = Parent as Window ?? throw new NullReferenceException();
            window.Close();
        }
    }
}
