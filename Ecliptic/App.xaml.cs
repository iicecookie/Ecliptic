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
        // public const string DBFILENAME = "friendsapp.db";
        public App()
        {
            InitializeComponent();

            using (var db = new ApplicationContext())
            {
                // Удаляем бд, если она существуеты
                db.Database.EnsureDeleted();
                // Создаем бд, если она отсутствует
                db.Database.EnsureCreated();

                if (db.Buildings.Count() == 0)
                {
                    db.Buildings.Add(new Building { Name = "Tom",   Email = "tom@gmail.com",   Phone = "+1234567" });
                    db.Buildings.Add(new Building { Name = "Alice", Email = "alice@gmail.com", Phone = "+3435957" });
                  
                    db.SaveChanges();
                }
            }
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
