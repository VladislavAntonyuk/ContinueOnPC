using Android.App;
using Android.OS;

namespace ContinueOnPC;

[Activity (Theme = "@style/Maui.SplashTheme", Exported = true)]
[IntentFilter(actions: new string[]{ Android.Content.Intent.ActionSend }, Categories = new string[] { Android.Content.Intent.CategoryDefault }, DataMimeType = "text/plain")]
public class ShareActivity : MauiAppCompatActivity
{
    protected override async void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if (Android.Content.Intent.ActionSend == Intent.Action && Intent.Type == "text/plain")
        {
            var services = new ServiceCollection();
            services.AddSingleton<IPreferences>(Preferences.Default);
            services.AddSingleton<IDeviceInfo>(DeviceInfo.Current);
            services.AddSingleton<IPreferencesService, PreferencesService>();
            services.AddSingleton<IFirebaseService, FirebaseService>();
            var serviceProvider = services.BuildServiceProvider();
            var firebaseService = serviceProvider.GetRequiredService<IFirebaseService>();
            var url = Intent.GetStringExtra(Android.Content.Intent.ExtraText);
            await firebaseService.PublishDataAsync(url);
        }
    }
}

