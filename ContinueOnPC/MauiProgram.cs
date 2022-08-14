using CommunityToolkit.Maui;

namespace ContinueOnPC;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        builder.Services.AddSingleton<IPreferences>(Preferences.Default);
        builder.Services.AddSingleton<IDeviceInfo>(DeviceInfo.Current);
        builder.Services.AddSingleton<ILauncher>(Launcher.Default);
        builder.Services.AddSingleton<IPreferencesService, PreferencesService>();
        builder.Services.AddSingleton<IFirebaseService, FirebaseService>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();
        return builder.Build();
    }
}