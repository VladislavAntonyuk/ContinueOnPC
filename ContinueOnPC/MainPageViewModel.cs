namespace ContinueOnPC;

using System.Reactive.Disposables;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Services;

public partial class MainPageViewModel : ObservableObject
{
	private readonly IDispatcher dispatcher;
	private readonly IFirebaseService firebaseService;
	private readonly ILauncher launcher;
	private readonly IPreferencesService preferencesService;

	[ObservableProperty]
	private bool isSubscribed;

	private IDisposable? subscription;

	public MainPageViewModel(IPreferencesService preferencesService,
		IFirebaseService firebaseService,
		ILauncher launcher,
		IDispatcher dispatcher)
	{
		this.preferencesService = preferencesService;
		this.firebaseService = firebaseService;
		this.launcher = launcher;
		this.dispatcher = dispatcher;
	}

	public string DbUrl
	{
		get => preferencesService.Get(Constants.DbUrlKey);
		set
		{
			if (DbUrl == value)
			{
				return;
			}

			preferencesService.Save(Constants.DbUrlKey, value);
		}
	}

	public string Login
	{
		get => preferencesService.Get(Constants.LoginKey);
		set
		{
			if (Login == value)
			{
				return;
			}

			preferencesService.Save(Constants.LoginKey, value);
		}
	}

	public string Password
	{
		get => preferencesService.Get(Constants.PasswordKey);
		set
		{
			if (Password == value)
			{
				return;
			}

			preferencesService.Save(Constants.PasswordKey, value);
		}
	}

	public string WebApiKey
	{
		get => preferencesService.Get(Constants.WebApiKey);
		set
		{
			if (WebApiKey == value)
			{
				return;
			}

			preferencesService.Save(Constants.WebApiKey, value);
		}
	}

	public string LoginUrl
	{
		get => preferencesService.Get(Constants.LoginUrlKey);
		set
		{
			if (LoginUrl == value)
			{
				return;
			}

			preferencesService.Save(Constants.LoginUrlKey, value);
		}
	}

	[RelayCommand]
	private async Task Test()
	{
		var result = await firebaseService.PublishDataAsync("https://vladislavantonyuk.github.io");
		if (result.IsSuccessful)
		{
			await Toast.Make("Success").Show();
		}
		else
		{
			var errorMessage = string.Join(Environment.NewLine, result.Errors);
			if (Application.Current?.MainPage is not null)
			{
				await Application.Current.MainPage.DisplayAlert("Unable to send data. Check the input data",
				                                                errorMessage, "OK");
			}
		}
	}

	[RelayCommand]
	private async Task Subscribe()
	{
		if (IsSubscribed)
		{
			subscription?.Dispose();
			IsSubscribed = false;
		}
		else
		{
			IsSubscribed = true;
			subscription = await firebaseService.SubscribeDataAsync(async x =>
			{
				await dispatcher.DispatchAsync(() => launcher.OpenAsync(x.Link));
			}, async exception =>
			{
				await dispatcher.DispatchAsync(async () =>
				{
					if (Application.Current?.MainPage is not null)
					{
						var error = exception.InnerException is null ? exception.Message : exception.InnerException.Message;
						await Application.Current.MainPage.DisplayAlert("Unable to subscribe", $"Error: {error}", "OK");
					}

					IsSubscribed = false;
				});
			});
			if (Equals(subscription, Disposable.Empty))
			{
				IsSubscribed = false;
				await Toast.Make("Subscription is not active. Check the input data").Show();
			}
		}
	}
}