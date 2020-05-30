using Ecliptic.Repository;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ecliptic.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Ecliptic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DbService.RefrashDb(true);

            DbService.LoadAll();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
