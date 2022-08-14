using ContinueOnPC.Models;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Database.Query;

namespace ContinueOnPC;

public class FirebaseService : IFirebaseService
{
    IPreferencesService preferencesService;
    private readonly IDeviceInfo deviceInfo;

    public FirebaseService(IPreferencesService preferencesService, IDeviceInfo deviceInfo)
    {
        this.preferencesService = preferencesService;
        this.deviceInfo = deviceInfo;
    }

    public async Task PublishDataAsync(string uri)
    {
        using var firebaseClient = await GetClient();
        await firebaseClient.Child("Links").PostAsync(new LinkInfo
        {
            Link = new Uri(uri),
            Source = deviceInfo.Name
        });
    }

    public async Task<IDisposable> SubscribeDataAsync(Func<LinkInfo, Task> action)
    {
        using var firebaseClient = await GetClient();
        return firebaseClient.Child("Links").AsObservable<LinkInfo>().Subscribe(async x =>
        {
            if (x.Object == null)
            {
                await firebaseClient.Child("Links").Child(x.Key).DeleteAsync();
                return;
            }

            if (x.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate && deviceInfo.Name != x.Object.Source)
            {
                await action(x.Object);
                await firebaseClient.Child("Links").Child(x.Key).DeleteAsync();
            }
        });
    }

    private async Task<FirebaseClient> GetClient()
    {
        var config = new FirebaseAuthConfig
        {
            ApiKey = preferencesService.Get(Constants.WebApiKey),
            AuthDomain = preferencesService.Get(Constants.LoginUrlKey),
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            }
        };

        var client = new FirebaseAuthClient(config);
        var userCredential = await client.SignInWithEmailAndPasswordAsync(preferencesService.Get(Constants.LoginKey), preferencesService.Get(Constants.PasswordKey));
        return new FirebaseClient(preferencesService.Get(Constants.DbUrlKey),
            new FirebaseOptions()
            {
                AuthTokenAsyncFactory = () => userCredential.User.GetIdTokenAsync()
            });
    }
}