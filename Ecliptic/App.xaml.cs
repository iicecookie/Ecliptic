using Ecliptic.Repository;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.IO;
using Ecliptic.Models;
using System.Linq;
using System.Collections.Generic;
using Ecliptic.Data;
using Microsoft.EntityFrameworkCore;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Ecliptic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DbService.RefrashDb();

            DbService.AddBuilding(new Building { Name = "Tom", Email = "tom@gmail.com", Phone = "+1234567" });
            DbService.AddBuilding(new Building { Name = "Alice", Email = "alice@gmail.com", Phone = "+3435957" });

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
