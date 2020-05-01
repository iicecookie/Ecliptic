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

            DbService.RefrashDb(false);

            DbService.AddBuilding(new Building { Name = "Tom", Email = "tom@gmail.com", Phone = "+1234567" });
            DbService.AddBuilding(new Building { Name = "Alice", Email = "alice@gmail.com", Phone = "+3435957" });


          //  DbService.LoadAll();    
            
            DbService.LoadSample();

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
