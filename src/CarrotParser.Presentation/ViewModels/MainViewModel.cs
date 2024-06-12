using CarrotParser.Application;
using CarrotParser.Application.Database;
using CarrotParser.Application.Model;
using CarrotParser.Application.Parser;
using CarrotParser.Presentation.Model;
using CarrotParser.Presentation.ViewModels.Common;
using CarrotParser.Presentation.ViewModels.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq.Expressions;
using System.Windows;

namespace CarrotParser.Presentation.ViewModels;

public class MainViewModel : ViewModelBase, IDisposable
{
    private const int PEOPLE_ON_PAGE = 10;

    private readonly IDialogService _dialogService;
    private readonly IDbManager _dbManager;
    private readonly IPersonsParser _personsParser;
    private readonly IConfiguration _configuration;

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
    public RelayCommand UpdatePersonCommand { get; private set; }
    public RelayCommand DeletePersonCommand { get; private set; }
    public RelayCommand MoveDatabaseCommand { get; private set; }
    public RelayCommand FindCommand { get; private set; }
    public RelayCommand LoadFirstPageCommand { get; private set; }

    public MainViewModel(IDialogService dialogService, IDbManager dbManager, IPersonsParser personsParser, IConfiguration configuration)
    {
        _dialogService = dialogService;
        _dbManager = dbManager;
        _personsParser = personsParser;
        _configuration = configuration;
        ShowConnectionStringWindow = new(OnShowConnectionStringWindowClick);
        NextPageCommand = new(OnNextPageCommandClick);
        PreviousPageCommand = new(OnPreviousPageCommandClick);
        DownloadOneCommand = new(OnDownloadOneCommandClick);
        DownloadManyCommand = new(OnDownloadManyCommandClick);
        StopCommand = new(OnStopCommand);
        UpdatePersonCommand = new(OnUpdatePersonCommand);
        DeletePersonCommand = new(OnDeletePersonCommand);
        MoveDatabaseCommand = new(OnMoveDatabaseCommand);
        FindCommand = new(OnFindCommand);
        LoadFirstPageCommand = new(OnLoadFirstPageCommand);
    }

    private void LoadFirstPageAndCheckForSecond()
    {
        PageNumber = 0;
        var repository = _dbManager.GetRepository();
        if (repository is null)
        {
            return;
        }
        _people = new(repository.Get(_pageNumber * PEOPLE_ON_PAGE, PEOPLE_ON_PAGE));
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
        _people = new(repository.Get(_pageNumber * PEOPLE_ON_PAGE, PEOPLE_ON_PAGE));
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
        if (!LoadPeopleAndReturnTrueIfSuccess())
        {
            MessageBox.Show("Can't load people.");
        }
    }

    private void OnPreviousPageCommandClick(object obj)
    {
        _pageNumber--;
        OnPropertyChanged(nameof(PageNumber));
        UpdatePagesFlags();
        if (!LoadPeopleAndReturnTrueIfSuccess())
        {
            MessageBox.Show("Can't load people.");
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

    private void OnUpdatePersonCommand(object obj)
    {
        _dialogService.ShowDialog<UpdatePersonDialogViewModel>((obj) => { });
    }

    private void OnDeletePersonCommand(object obj)
    {
        if (_selectedPerson is null)
        {
            return;
        }
        var repository = _dbManager.GetRepository();
        if (repository is null)
        {
            MessageBox.Show("Can't operate with database.");
            return;
        }
        repository.DeletePerson(_selectedPerson.Id);
        _people.Remove(_selectedPerson);
        OnPropertyChanged(nameof(People));
    }

    private void OnMoveDatabaseCommand(object obj)
    {
        _dialogService.ShowDialog<MoveDatabaseDialogViewModel>(MoveDatabaseCallback);
    }


    private void MoveDatabaseCallback(object newLocation)
    {
        string newPath = (string)newLocation;
        var oldPath = _configuration.GetValue<string>(DepedencyInjection.DB_CONFIGURATION_SECTION_NAME);
        if (oldPath is null)
        {
            MessageBox.Show("Can't find database.");
            return;
        }
        _dbManager.MoveDatabase(oldPath, newPath);
        MessageBox.Show($"Database moved to {newPath}.");
    }

    private void OnFindCommand(object obj)
    {
        _dialogService.ShowDialog<FindSelectorDialogViewModel>(FindBySelector);
    }

    private void FindBySelector(object selectorObj)
    {
        var selector = (FindSelector)selectorObj;
        var repository = _dbManager.GetRepository();
        if (repository is null)
        {
            MessageBox.Show("Can't operate with database.");
            return;
        }
        _pageNumber = 0;
        // if not setted
        if (selector.Username is null && selector.Email is null && (selector.DateTimeTo is null || selector.DateTimeSince is null))
        {
            LoadFirstPageAndCheckForSecond();
            return;
        }
        List<Person> people = [];
        if (selector.Username is not null)
        {
            people = repository.GetPersonByUsername(selector.Username);
        }
        if (selector.Email is not null)
        {
            if (people.Count == 0)
            {
                people = repository.GetPersonsByEmail(selector.Email);
            }
            else
            {
                people = people.Where(p => p.Email == selector.Email).ToList();
            }
        }
        if (selector.DateTimeSince is not null && selector.DateTimeTo is not null)
        {
            if (people.Count == 0)
            {
                people = repository.GetBetweenDateTimes(selector.DateTimeSince.Value.Date, selector.DateTimeTo.Value.Date);
            }
            else
            {
                people = people.Where(p => p.Id.CreationTime.Date >= selector.DateTimeSince.Value.Date
                    && p.Id.CreationTime.Date >= selector.DateTimeSince.Value.Date).ToList();
            }
        }
        People = new(people);
    }

    private void OnLoadFirstPageCommand(object obj)
    {
        LoadFirstPageAndCheckForSecond();
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

    public int PageNumber
    {
        get { return _pageNumber + 1; }
        set
        {
            _pageNumber = value;
            OnPropertyChanged(nameof(PageNumber));
        }
    }

    public bool HasMorePages => _hasMorePages;

    public bool IsNotFirstPage => _pageNumber != 0;
}
