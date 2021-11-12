using System;
using System.Windows.Input;
using ContinueOnPC.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace ContinueOnPC
{
    public class MainPageViewModel:ObservableObject
    {
        IPreferencesService preferencesService;
        bool isSubscribed = false;
        IDisposable subscription;
        public MainPageViewModel(IPreferencesService preferencesService, IFirebaseService firebaseService, ILauncher launcher)
        {
            this.preferencesService = preferencesService;
            TestCommand = new Command(async () =>
            {
                await firebaseService.PublishDataAsync("https://vladislavantonyuk.azurewebsites.net");
            });
            SubscribeCommand = new Command(async () =>
            {
                if (IsSubscribed)
                {
                    subscription.Dispose();
                    IsSubscribed = false;
                }
                else
                {
                    IsSubscribed = true;
                    subscription = await firebaseService.SubscribeDataAsync(async x =>
                    {
                        await Device.InvokeOnMainThreadAsync(async () =>
                        {
                            await launcher.OpenAsync(x.Link);
                        });
                    });
                }                
            });
        }

        public bool IsSubscribed
        {
            get => isSubscribed;
            set => SetProperty(ref isSubscribed, value);
        }

        public string DbUrl
        {
            get => preferencesService.Get(Constants.DbUrlKey);
            set
            {
                if (DbUrl == value)
                    return;

                preferencesService.Save(Constants.DbUrlKey, value);
            }
        }


        public string Login
        {
            get => preferencesService.Get(Constants.LoginKey);
            set
            {
                if (Login == value)
                    return;

                preferencesService.Save(Constants.LoginKey, value);
            }
        }

        public string Password
        {
            get => preferencesService.Get(Constants.PasswordKey);
            set
            {
                if (Password == value)
                    return;

                preferencesService.Save(Constants.PasswordKey, value);
            }
        }

        public string WebApiKey
        {
            get => preferencesService.Get(Constants.WebApiKey);
            set
            {
                if (WebApiKey == value)
                    return;

                preferencesService.Save(Constants.WebApiKey, value);
            }
        }

        public string LoginUrl
        {
            get => preferencesService.Get(Constants.LoginUrlKey);
            set
            {
                if (LoginUrl == value)
                    return;

                preferencesService.Save(Constants.LoginUrlKey, value);
            }
        }

        public ICommand TestCommand { get; }
        public ICommand SubscribeCommand { get; }
    }
}

