using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace ContinueOnPC
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = ((App)Application.Current).ServiceProvider.GetService<MainPageViewModel>();
        }
    }
}

