using CarrotParser.Application.Crypto;
using CarrotParser.Application.Database;
using CarrotParser.Application.Model;
using CarrotParser.Presentation.ViewModels.Common;
using CarrotParser.Presentation.ViewModels.Common.Interfaces;
using System.Security.Authentication;
using System.Windows;

namespace CarrotParser.Presentation.ViewModels;

public class UpdatePersonDialogViewModel : ViewModelBase, IDialogViewModel
{
    private readonly IHashProvider _hashProvider;
    private readonly IDbManager _dbManager;
    private Person _person;

    public event Action<object>? OnResult;

    public RelayCommand SaveUpdatesCommand { get; private set; }

    public UpdatePersonDialogViewModel(IHashProvider hashProvider, MainViewModel mainViewModel, IDbManager dbManager)
    {
        _hashProvider = hashProvider;
        _person = mainViewModel.SelectedPerson!;
        _dbManager = dbManager;
        SaveUpdatesCommand = new(OnSaveUpdatesCommand);
    }

    private void OnSaveUpdatesCommand(object obj)
    {
        SetPassordProperties();
        var repository = _dbManager.GetRepository();
        if (repository is null)
        {
            MessageBox.Show("Can't operate with database.");
            return;
        }
        repository.UpdatePerson(Person);
    }

    private void SetPassordProperties()
    {
        var password = _person.Login.Password;
        _person.Login.Md5 = _hashProvider.Hash(password, _person.Login.Salt, HashAlgorithmType.Md5);
        _person.Login.Sha1 = _hashProvider.Hash(password, _person.Login.Salt, HashAlgorithmType.Sha1);
        _person.Login.Sha256 = _hashProvider.Hash(password, _person.Login.Salt, HashAlgorithmType.Sha256);
    }

    public Person Person
    {
        get { return _person; }
        set
        {
            _person = value;
            OnPropertyChanged(nameof(Person));
        }
    }
}
