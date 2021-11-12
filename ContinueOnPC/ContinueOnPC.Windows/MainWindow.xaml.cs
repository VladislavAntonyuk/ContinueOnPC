using Microsoft.Extensions.DependencyInjection;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;
namespace ContinueOnPC.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FormsApplicationPage
{
    public MainWindow()
    {
        InitializeComponent();
        Forms.Init();
        var services = new ServiceCollection();
        services.AddSingleton<IPreferences, NetPreferencesImplementation>();
        services.AddSingleton<ILauncher, NetLauncherImplementation>();
        services.AddSingleton<IDeviceInfo, NetDeviceInfoImplementation>();
        LoadApplication(new ContinueOnPC.App(services));
    }
}
