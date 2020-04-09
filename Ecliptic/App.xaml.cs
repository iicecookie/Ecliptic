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

            using (var dob = new ApplicationContext())
            {
                ApplicationContext.db.Database.EnsureDeleted();

                // Создаем бд, если она отсутствует
                ApplicationContext.db.Database.EnsureCreated();

                if (ApplicationContext.db.Buildings.Count() == 0)
                {
                    ApplicationContext.db.Buildings.Add(new Building { Name = "Tom", Email = "tom@gmail.com", Phone = "+1234567" });
                    ApplicationContext.db.Buildings.Add(new Building { Name = "Alice", Email = "alice@gmail.com", Phone = "+3435957" });
                    /*
                    var r1 = new Room
                    {
                        Name = "213",
                        Floor = 1,
                        Details = "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                        Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                        Phone = "8(906)6944309",
                        Timetable = "Время работы:вторник	  10:00–22:00" +
                "             среда       10:00–22:00" +
                "             четверг     10:00–22:00" +
                "             пятница     10:00–22:00" +
                "             суббота     10:00–22:00" +
                "             воскресенье 10:00–22:00" +
                "             понедельник 10:00–22:00",

                    };
                    var r2 = new Room
                    {
                        Name = "200",
                        Floor = 1,
                        Details = "The Asian black bear, also known as the moon bear and the white-chested bear, is a medium-sized bear species native to Asia and largely adapted to arboreal life. It lives in the Himalayas, in the northern parts of the Indian subcontinent, Korea, northeastern China, the Russian Far East, the Honshū and Shikoku islands of Japan, and Taiwan. It is classified as vulnerable by the International Union for Conservation of Nature (IUCN), mostly because of deforestation and hunting for its body parts.",
                        Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",

                    };
                    var r3 = new Room
                    {
                        Name = "202",
                        Floor = 2,
                        Details = "The brown bear is a bear that is found across much of northern Eurasia and North America. In North America the population of brown bears are often called grizzly bears. It is one of the largest living terrestrial members of the order Carnivora, rivaled in size only by its closest relative, the polar bear, which is much less variable in size and slightly larger on average. The brown bear's principal range includes parts of Russia, Central Asia, China, Canada, the United States, Scandinavia and the Carpathian region, especially Romania, Anatolia and the Caucasus. The brown bear is recognized as a national and state animal in several European countries.",

                    };

                    ApplicationContext.db.Rooms.Add(r1);
                    ApplicationContext.db.Rooms.Add(r2);
                    ApplicationContext.db.Rooms.Add(r3);

                //    db.SaveChanges();

                    var w1 = new Worker
                    {
                        FirstName = "Celivans",
                        SecondName = "irina",
                        LastName = "vasileva",

                        Details = "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                        Status = "Teacher",

                        Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                        Phone = "8(906)6944309",
                        Email = "seliv@mail.ru",
                        Room = r1,

                    };
                    var w2 = new Worker
                    {
                        FirstName = "Uraeva",
                        SecondName = "Elena",

                        Details = "The American black    bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                        Status = "Teacher",

                        Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                        Phone = "8(906)6944309",
                        Email = "seliv@mail.ru",
                        Room = r2,
                    };
                    var w3 = new Worker
                    {
                        FirstName = "Makarov",
                        SecondName = "Kirya",

                        Details = "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                        Status = "Teacher",

                        Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                        Phone = "8(906)6944309",
                        Email = "seliv@mail.ru",
                    };

                    ApplicationContext.db.Workers.Add(w1);
                    ApplicationContext.db.Workers.Add(w2);
                    ApplicationContext.db.Workers.Add(w3);
                    */
                    ApplicationContext.db.SaveChanges();
                }
                /*
               RoomData.Rooms = ApplicationContext.db.Rooms
                              .Include(d => d.Workers)
                              .ToList();

                var i = WorkerData.Workers;
                var j = RoomData.Rooms;

                Room company = ApplicationContext.db.Rooms.FirstOrDefault();
                ApplicationContext.db.Entry(company).Collection(t => t.Workers).Load();
                */

                int e = 4;
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
