using CarrotParser.Application.Database;
using CarrotParser.Application.Model;
using CarrotParser.Presentation.ViewModels.Common;
using CarrotParser.Presentation.ViewModels.Common.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace CarrotParser.Presentation.ViewModels;

public class MainViewModel : ViewModelBase
{
    private const int PEOPLE_ON_PAGE = 100;

    private readonly IDialogService _dialogService;
    private readonly IDbManager _dbManager;

    private ObservableCollection<Person>? _people = [];
    private Person? _selectedPerson;
    private int _pageNumber = 0;
    private bool _hasMorePages = false;

    public RelayCommand ShowConnectionStringWindow { get; private set; }
    public RelayCommand NextPageCommand { get; private set; }
    public RelayCommand PreviousPageCommand { get; private set; }

    public MainViewModel(IDialogService dialogService, IDbManager dbManager)
    {
        _dialogService = dialogService;
        _dbManager = dbManager;
        LoadFirstPageAndCheckSecond();
        ShowConnectionStringWindow = new(OnShowConnectionStringWindowClick);
        NextPageCommand = new(OnNextPageCommandClick);
        PreviousPageCommand = new(OnPreviousPageCommandClick);
    }

    private void LoadFirstPageAndCheckSecond()
    {
        var repository = _dbManager.GetRepository();
        if (repository is null)
        {
            return;
        }
        _people = new(repository.Get(_pageNumber, PEOPLE_ON_PAGE));
        OnPropertyChanged(nameof(People));
        if (repository.Get(_pageNumber + 1, PEOPLE_ON_PAGE).Count != 0)
        {
            _hasMorePages = true;
            OnPropertyChanged(nameof(HasMorePages));
        }
    }

    private bool LoadPeopleAndReturnTrueIfSuccess()
    {
        var repository = _dbManager.GetRepository();
        if (repository is null)
        {
            return false;
        }
        _people = new(repository.Get(_pageNumber, PEOPLE_ON_PAGE));
        OnPropertyChanged(nameof(People));
        return true;
    }

    private void OnShowConnectionStringWindowClick(object obj)
    {
        _dialogService.ShowDialog<ConnectionStringDialogViewModel>(ConnectionStringSettedCallback);
    }

    private void ConnectionStringSettedCallback(object connectionString)
    {
        if (connectionString is not string)
        {
            throw new ArgumentException($"Invalid type {connectionString.GetType().FullName}, string expected.", nameof(connectionString));
        }
        try
        {
            _dbManager.CreateDatabase((string)connectionString);
            if (_dbManager.GetRepository() is null)
            {
                MessageBox.Show($"Can't connect to database with connection string {connectionString}.");
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void OnNextPageCommandClick(object obj)
    {
        _pageNumber++;
        OnPropertyChanged(nameof(PageNumber));
        if (!LoadPeopleAndReturnTrueIfSuccess())
        {
            MessageBox.Show("Can't load persons.");
        }
    }

    private void OnPreviousPageCommandClick(object obj)
    {
        _pageNumber--;
        OnPropertyChanged(nameof(PageNumber));
        if (!LoadPeopleAndReturnTrueIfSuccess())
        {
            MessageBox.Show("Can't load persons.");
        }
    }

    public ObservableCollection<Person>? People
    {
        get => _people;
        set
        {
            _people = value;
            OnPropertyChanged(nameof(People));
        }
    }

    public Person? SelectedPerson
    {
        get => _selectedPerson;
        set
        {
            _selectedPerson = value;
            OnPropertyChanged(nameof(SelectedPerson));
        }
    }

    public int PageNumber => _pageNumber;

    public bool HasMorePages => _hasMorePages;

    public bool IsNotFirstPage => _pageNumber != 0;
}
