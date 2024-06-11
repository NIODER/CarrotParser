using CarrotParser.Application.Database;
using CarrotParser.Application.Model;
using CarrotParser.Application.Parser;
using CarrotParser.Presentation.ViewModels.Common;
using CarrotParser.Presentation.ViewModels.Common.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace CarrotParser.Presentation.ViewModels;

public class MainViewModel : ViewModelBase, IDisposable
{
    private const int PEOPLE_ON_PAGE = 10;

    private readonly IDialogService _dialogService;
    private readonly IDbManager _dbManager;
    private readonly IPersonsParser _personsParser;

    private ObservableCollection<Person> _people = [];
    private Person? _selectedPerson;
    private int _pageNumber = 0;
    private bool _hasMorePages = false;

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public RelayCommand ShowConnectionStringWindow { get; private set; }
    public RelayCommand NextPageCommand { get; private set; }
    public RelayCommand PreviousPageCommand { get; private set; }
    public RelayCommand DownloadOneCommand { get; private set; }
    public RelayCommand DownloadManyCommand { get; private set; }
    public RelayCommand StopCommand { get; private set; }

    public MainViewModel(IDialogService dialogService, IDbManager dbManager, IPersonsParser personsParser)
    {
        _dialogService = dialogService;
        _dbManager = dbManager;
        _personsParser = personsParser;
        ShowConnectionStringWindow = new(OnShowConnectionStringWindowClick);
        NextPageCommand = new(OnNextPageCommandClick);
        PreviousPageCommand = new(OnPreviousPageCommandClick);
        DownloadOneCommand = new(OnDownloadOneCommandClick);
        DownloadManyCommand = new(OnDownloadManyCommandClick);
        StopCommand = new(OnStopCommand);
    }

    private void LoadFirstPageAndCheckForSecond()
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
            LoadFirstPageAndCheckForSecond();
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
        UpdatePagesFlags();
    }

    private void OnPreviousPageCommandClick(object obj)
    {
        _pageNumber--;
        OnPropertyChanged(nameof(PageNumber));
        UpdatePagesFlags();
        if (!LoadPeopleAndReturnTrueIfSuccess())
        {
            MessageBox.Show("Can't load persons.");
        }
    }

    private void UpdatePagesFlags()
    {
        OnPropertyChanged(nameof(IsNotFirstPage));
        OnPropertyChanged(nameof(HasMorePages));
        if (_people.Count < 10)
        {
            _hasMorePages = false;
            OnPropertyChanged(nameof(HasMorePages));
        }
        else
        {
            _hasMorePages = true;
        }
    }

    private async void OnDownloadOneCommandClick(object obj)
    {
        var repository = _dbManager.GetRepository();
        if (repository is null)
        {
            MessageBox.Show("Can't operate with database.");
            return;
        }
        try
        {
            var person = await _personsParser.GetPersonAsync(_cancellationTokenSource.Token);
            repository.CreatePerson(person);
            _people.Add(person);
            OnPropertyChanged(nameof(People));
        }
        catch (TaskCanceledException)
        {
            MessageBox.Show("Downloading cancelled.");
        }
    }

    private async void OnDownloadManyCommandClick(object obj)
    {
        var repository = _dbManager.GetRepository();
        if (repository is null)
        {
            MessageBox.Show("Can't operate with database.");
            return;
        }
        try
        {
            await foreach (var person in _personsParser.GetPersonsAsync(100, _cancellationTokenSource.Token))
            {
                repository.CreatePerson(person);
                _people.Add(person);
                OnPropertyChanged(nameof(People));
            }
        }
        catch (TaskCanceledException)
        {
            MessageBox.Show("Downloading cancelled.");
        }
    }

    private void OnStopCommand(object obj)
    {
        _cancellationTokenSource.Cancel();
    }

    public void Dispose()
    {
        _dbManager.Dispose();
    }

    public ObservableCollection<Person> People
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
