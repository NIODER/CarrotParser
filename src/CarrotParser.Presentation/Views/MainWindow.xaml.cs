using CarrotParser.Presentation.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace CarrotParser.Presentation.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        var viewModel = DataContext as MainViewModel;
        if (viewModel is not null)
        {
            viewModel.Dispose();
        }
        base.OnClosing(e);
    }
}
