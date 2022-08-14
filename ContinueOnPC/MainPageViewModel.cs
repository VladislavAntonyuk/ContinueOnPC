using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContinueOnPC.Models;

namespace ContinueOnPC;

public partial class MainPageViewModel:ObservableObject
{
    private readonly IPreferencesService _preferencesService;
    private readonly IFirebaseService _firebaseService;
    private readonly ILauncher _launcher;
    private readonly IDispatcher _dispatcher;

    [ObservableProperty]
    bool isSubscribed;
        
    IDisposable subscription;
    public MainPageViewModel(IPreferencesService preferencesService,
        IFirebaseService firebaseService,
        ILauncher launcher,
        IDispatcher dispatcher)
    {
        _preferencesService = preferencesService;
        _firebaseService = firebaseService;
        _launcher = launcher;
        _dispatcher = dispatcher;
    }

    public string DbUrl
    {
        get => _preferencesService.Get(Constants.DbUrlKey, Constants.DbUrlValue);
        set
        {
            if (DbUrl == value)
                return;

            _preferencesService.Save(Constants.DbUrlKey, value);
        }
    }
        
    public string Login
    {
        get => _preferencesService.Get(Constants.LoginKey, Constants.LoginValue);
        set
        {
            if (Login == value)
                return;

            _preferencesService.Save(Constants.LoginKey, value);
        }
    }

    public string Password
    {
        get => _preferencesService.Get(Constants.PasswordKey, Constants.PasswordValue);
        set
        {
            if (Password == value)
                return;

            _preferencesService.Save(Constants.PasswordKey, value);
        }
    }

    public string WebApiKey
    {
        get => _preferencesService.Get(Constants.WebApiKey, Constants.WebApiValue);
        set
        {
            if (WebApiKey == value)
                return;

            _preferencesService.Save(Constants.WebApiKey, value);
        }
    }

    public string LoginUrl
    {
        get => _preferencesService.Get(Constants.LoginUrlKey, Constants.LoginUrlValue);
        set
        {
            if (LoginUrl == value)
                return;

            _preferencesService.Save(Constants.LoginUrlKey, value);
        }
    }

    [RelayCommand]
    async Task Test()
    {
        await _firebaseService.PublishDataAsync("https://vladislavantonyuk.azurewebsites.net");
        await Toast.Make("Success").Show();
    }

    [RelayCommand]
    async Task Subscribe()
    {
        if (IsSubscribed)
        {
            subscription.Dispose();
            IsSubscribed = false;
        }
        else
        {
            IsSubscribed = true;
            subscription = await _firebaseService.SubscribeDataAsync(async x =>
            {
                await _dispatcher.DispatchAsync(() => _launcher.OpenAsync(x.Link));
            });
        }            
    }
}