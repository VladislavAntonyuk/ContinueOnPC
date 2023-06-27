namespace ContinueOnPC;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

public partial class MainPageViewModel : ObservableObject
{
	private readonly IDispatcher _dispatcher;
	private readonly IFirebaseService _firebaseService;
	private readonly ILauncher _launcher;
	private readonly IPreferencesService _preferencesService;

	[ObservableProperty]
	private bool isSubscribed;

	private IDisposable subscription;

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
		get => _preferencesService.Get(Constants.DbUrlKey);
		set
		{
			if (DbUrl == value)
			{
				return;
			}

			_preferencesService.Save(Constants.DbUrlKey, value);
		}
	}

	public string Login
	{
		get => _preferencesService.Get(Constants.LoginKey);
		set
		{
			if (Login == value)
			{
				return;
			}

			_preferencesService.Save(Constants.LoginKey, value);
		}
	}

	public string Password
	{
		get => _preferencesService.Get(Constants.PasswordKey);
		set
		{
			if (Password == value)
			{
				return;
			}

			_preferencesService.Save(Constants.PasswordKey, value);
		}
	}

	public string WebApiKey
	{
		get => _preferencesService.Get(Constants.WebApiKey);
		set
		{
			if (WebApiKey == value)
			{
				return;
			}

			_preferencesService.Save(Constants.WebApiKey, value);
		}
	}

	public string LoginUrl
	{
		get => _preferencesService.Get(Constants.LoginUrlKey);
		set
		{
			if (LoginUrl == value)
			{
				return;
			}

			_preferencesService.Save(Constants.LoginUrlKey, value);
		}
	}

	[RelayCommand]
	private async Task Test()
	{
		await _firebaseService.PublishDataAsync("https://vladislavantonyuk.azurewebsites.net");
		await Toast.Make("Success").Show();
	}

	[RelayCommand]
	private async Task Subscribe()
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