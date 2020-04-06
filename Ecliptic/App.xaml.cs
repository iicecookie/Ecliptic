using Ecliptic.Repository;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.IO;
using Ecliptic.Models;
using System.Linq;
using System.Collections.Generic;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Ecliptic
{
    public partial class App : Application
    {
        public const string DBFILENAME = "friendsapp.db";
        public App()
        {
            InitializeComponent();

            using (var db = new ApplicationContext())
            {
           //     db.Database.EnsureDeleted();

                // Создаем бд, если она отсутствует
                db.Database.EnsureCreated();

                if (db.Buildings.Count() == 0)
                {
                    db.Buildings.Add(new Building { Name = "Tom", Email = "tom@gmail.com", Phone = "+1234567" });
                    db.Buildings.Add(new Building { Name = "Alice", Email = "alice@gmail.com", Phone = "+3435957" });
                   
                    db.SaveChanges();
                }
            }


            MainPage = new AppShell();
        }


        protected override void OnStart()
        {
            // Handle when your app starts


            // string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            using (var db = new ApplicationContext())
            {
                // Создаем бд, если она отсутствуе
                
               // db.Database.EnsureCreated();
             //   if (db.User.Count() != 0)
             //   {
             //       User.setInstance(db.User.First());
             //   }
            }
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
