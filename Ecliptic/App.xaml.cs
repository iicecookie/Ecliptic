using Ecliptic.Repository;
using Ecliptic.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Ecliptic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DbService.RefrashDb(false);

            DbService.LoadAll();

            MainPage = new AppShell();
        }

        async protected override void OnStart()
        {
           // await BuildingPage.LoadBuildingsAsync();
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
