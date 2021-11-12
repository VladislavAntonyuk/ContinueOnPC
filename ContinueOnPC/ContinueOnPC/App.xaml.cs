using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;

namespace ContinueOnPC
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; }
        public App(ServiceCollection services)
        {
            InitializeComponent();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            MainPage = new MainPage();
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPreferencesService, PreferencesService>();
            serviceCollection.AddSingleton<IFirebaseService, FirebaseService>();
            serviceCollection.AddSingleton<MainPageViewModel>();
        }
    }
}

