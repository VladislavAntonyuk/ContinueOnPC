﻿namespace ContinueOnPC.Services;

using System.Reactive.Disposables;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Models;

public class FirebaseService : IFirebaseService
{
	private readonly IDeviceInfo deviceInfo;
	private readonly IPreferencesService preferencesService;

	public FirebaseService(IPreferencesService preferencesService, IDeviceInfo deviceInfo)
	{
		this.preferencesService = preferencesService;
		this.deviceInfo = deviceInfo;
	}

	public async Task<HasErrorResult<bool>> ValidateConnection()
	{
		try
		{
			using var firebaseClient = await GetClient();
			return new HasErrorResult<bool>().WithResult(true);
		}
		catch (FirebaseAuthHttpException e)
		{
			return new HasErrorResult<bool>().WithError(e.Reason.ToString());
		}
		catch (Exception e)
		{
			return new HasErrorResult<bool>().WithError(e.Message);
		}
	}

	public async Task<HasErrorResult<bool>> PublishDataAsync(string uri)
	{
		var isValid = await ValidateConnection();
		if (!isValid.IsSuccessful)
		{
			return isValid;
		}

		using var firebaseClient = await GetClient();
		await firebaseClient.Child("Links")
		                    .PostAsync(new LinkInfo
		                    {
			                    Link = new Uri(uri),
			                    Source = deviceInfo.Name
		                    });
		return isValid;
	}

	public async Task<IDisposable> SubscribeDataAsync(Func<LinkInfo, Task> action)
	{
		var isValid = await ValidateConnection();
		if (!isValid.IsSuccessful)
		{
			return Disposable.Empty;
		}

		var firebaseClient = await GetClient();
		return firebaseClient.Child("Links")
		                     .AsObservable<LinkInfo>()
		                     .Subscribe(async x =>
		                     {
			                     if (x.Object == null)
			                     {
				                     await firebaseClient.Child("Links").Child(x.Key).DeleteAsync();
				                     return;
			                     }

			                     if (x.EventType == FirebaseEventType.InsertOrUpdate &&
			                         deviceInfo.Name != x.Object.Source)
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
		var userCredential = await client.SignInWithEmailAndPasswordAsync(preferencesService.Get(Constants.LoginKey),
		                                                                  preferencesService.Get(
			                                                                  Constants.PasswordKey));
		return new FirebaseClient(preferencesService.Get(Constants.DbUrlKey), new FirebaseOptions
		{
			AuthTokenAsyncFactory = () => userCredential.User.GetIdTokenAsync()
		});
	}
}