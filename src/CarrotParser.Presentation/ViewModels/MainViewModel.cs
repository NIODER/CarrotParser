using CarrotParser.Presentation.ViewModels.Common;
using CarrotParser.Presentation.ViewModels.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotParser.Presentation.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IWindowService _windowService;

    public RelayCommand ShowConnectionStringWindow { get; private set; }

    public MainViewModel(IWindowService windowService)
    {
        _windowService = windowService;
        ShowConnectionStringWindow = new(OnShowConnectionStringWindowClick);
    }

    private void OnShowConnectionStringWindowClick(object obj)
    {
        _windowService.Show<ConnectionStringViewModel>();
    }
}
