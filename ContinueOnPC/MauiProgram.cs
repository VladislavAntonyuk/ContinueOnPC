namespace ContinueOnPC;

using CommunityToolkit.Maui;
using Services;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>().UseMauiCommunityToolkit();
		builder.Services.AddSingleton(Preferences.Default);
		builder.Services.AddSingleton(DeviceInfo.Current);
		builder.Services.AddSingleton(Launcher.Default);
		builder.Services.AddSingleton<IPreferencesService, PreferencesService>();
		builder.Services.AddSingleton<IFirebaseService, FirebaseService>();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();
		return builder.Build();
	}
}