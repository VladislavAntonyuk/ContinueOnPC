namespace ContinueOnPC;

using Android.App;
using Android.Content;
using Android.OS;

[Activity(Theme = "@style/Maui.SplashTheme", Exported = true)]
[IntentFilter(new[]
{
	Intent.ActionSend
}, Categories = new[]
{
	Intent.CategoryDefault
}, DataMimeType = "text/plain")]
public class ShareActivity : MauiAppCompatActivity
{
	protected override async void OnCreate(Bundle savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		if (Intent is null)
		{
			return;
		}

		if (Intent.Action == Intent.ActionSend && Intent.Type == "text/plain")
		{
			var services = new ServiceCollection();
			services.AddSingleton(Preferences.Default);
			services.AddSingleton(DeviceInfo.Current);
			services.AddSingleton<IPreferencesService, PreferencesService>();
			services.AddSingleton<IFirebaseService, FirebaseService>();
			var serviceProvider = services.BuildServiceProvider();
			var firebaseService = serviceProvider.GetRequiredService<IFirebaseService>();
			var url = Intent.GetStringExtra(Intent.ExtraText);
			await firebaseService.PublishDataAsync(url);
		}
	}
}