using Ecliptic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic.Models
{
    public class User
    {
        public static User CurrentUser { get; private set; }

        #region Params
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }

        public List<Note> Notes { get; set; }

        public List<Room> Favorites { get; set; }

        public static bool isNull
        {
            get
            {
                if (CurrentUser == null)
                    return true;
                return false;
            }
        }
        #endregion

        #region Constructors
        private User()
        {
            Notes = new List<Note>();
            Favorites = new List<Room>();
        }
        private User(string Name, string login, string pass) : this()
        {
            this.Name = Name;
            this.Login = login;
            this.Password = pass;
        }
        #endregion

        public static User getInstance()
        {
            return CurrentUser;
        }

        public static void setInstance(string name, string login, string pass)
        {
            CurrentUser = new User(name, login, pass);
        }

        public static bool CheckUser(string login, string password)
        {
            if (true)
            {

                return true;
            }

            return false;
        }

        public static void LoadUser(string login, string password)
        {
            // загружаем данные из базы

            // записываю в пользователя
            setInstance("Jo", login, password);

            // также загружаются заметки в NoteData
            // foreach
            // NoteData.AddNote(note);

            User.CurrentUser.Notes.Add(new Note(CurrentUser, "заметка1", "409", "KGU", true));
            User.CurrentUser.Notes.Add(new Note(CurrentUser, "заметка2", "213", "KGU", false));
            User.CurrentUser.Notes.Add(new Note(CurrentUser, "заметка3", "200", "KGU", false));
            User.CurrentUser.Notes.Add(new Note(CurrentUser, "заметка4", "200", "KGU", false));
            User.CurrentUser.Notes.Add(new Note(CurrentUser, "заметка5", "202", "KGU", false));
            User.CurrentUser.Notes.Add(new Note(CurrentUser, "заметка6", "202", "KGU", false));

            CurrentUser.Favorites.Add(new Room
            {
                Name = "213",
                Floor = 1,
                Staff = new List<Worker>
                {
                     WorkerData.GetWorker("Makarov","Kirya"),
                     WorkerData.GetWorker("Uraeva","Elena"),
                },

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

            });
            CurrentUser.Favorites.Add(new Room
            {
                Name = "200",
                Floor = 1,
                Details = "The Asian black bear, also known as the moon bear and the white-chested bear, is a medium-sized bear species native to Asia and largely adapted to arboreal life. It lives in the Himalayas, in the northern parts of the Indian subcontinent, Korea, northeastern China, the Russian Far East, the Honshū and Shikoku islands of Japan, and Taiwan. It is classified as vulnerable by the International Union for Conservation of Nature (IUCN), mostly because of deforestation and hunting for its body parts.",
                Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",

            });
            CurrentUser.Favorites.Add(new Room
            {
                Name = "202",
                Floor = 2,
                Details = "The brown bear is a bear that is found across much of northern Eurasia and North America. In North America the population of brown bears are often called grizzly bears. It is one of the largest living terrestrial members of the order Carnivora, rivaled in size only by its closest relative, the polar bear, which is much less variable in size and slightly larger on average. The brown bear's principal range includes parts of Russia, Central Asia, China, Canada, the United States, Scandinavia and the Carpathian region, especially Romania, Anatolia and the Caucasus. The brown bear is recognized as a national and state animal in several European countries.",

            });
            CurrentUser.Favorites.Add(new Room
            {
                Name = "201",
                Floor = 2,
                Details = "The giant panda, also known as panda bear or simply panda, is a bear native to south central China. It is easily recognized by the large, distinctive black patches around its eyes, over the ears, and across its round body. The name giant panda is sometimes used to distinguish it from the unrelated red panda. Though it belongs to the order Carnivora, the giant panda's diet is over 99% bamboo. Giant pandas in the wild will occasionally eat other grasses, wild tubers, or even meat in the form of birds, rodents, or carrion. In captivity, they may receive honey, eggs, fish, yams, shrub leaves, oranges, or bananas along with specially prepared food.",

            });
        }
        public static void LoginOut()
        {
            CurrentUser = null;
        }

        // TODO with server part
        public static void AddNote(Note note)
        {
            CurrentUser.Notes.Add(note);
            // если note ispublic == true -> отправить на сервер
        }

        // TODO with server part
        public static string DeleteNote(string id)
        {
            // удалить с сервера если общая
            for (int i = 0; i < CurrentUser.Notes.Count; i++)
            {
                if (CurrentUser.Notes[i].Id == id)
                {
                    if (!CurrentUser.Notes.Remove(CurrentUser.Notes[i]))
                    {
                        return "Не получилось";
                    }
                    return "deleted";
                }
            }
            return "not this note";
        }

        // Мысль - обсудить с Ураевой, возможно все заметки хранить на сервере
        // но те, что общие, добавлять именно к аудитории


        public static bool isRoomFavoit(Room room)
        {
            foreach(var fav in CurrentUser.Favorites)
            {
                if (fav.Name == room.Name && fav.Description == room.Description)
                {
                    return true;
                }
            }
            return false;
        }
    }
}