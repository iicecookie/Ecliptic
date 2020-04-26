using Ecliptic.Data;
using Ecliptic.Repository;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class User
    {
        public int UserId { get; set; }

        public static User CurrentUser { get; private set; }

        #region Params
        public string Name     { get; private set; }
        public string Login    { get; private set; }
        public string Password { get; private set; }

        public virtual List<Note> Notes     { get; set; }

        public virtual List<Room> Favorites { get; set; }

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
            Name = "";
            Login = "";
            Password = "";
            Notes     = new List<Note>();
            Favorites = new List<Room>();
        }
        private User(string Name, string login, string pass) : this()
        {
            this.Name = Name;
            this.Login = login;
            this.Password = pass;
        }

        public static User getInstance()
        {
            return CurrentUser;
        }

        public static void setInstance(User user)
        {
            CurrentUser = user;

        }

        public static void setInstance(string name, string login, string pass)
        {
            CurrentUser = new User(name, login, pass);
        }

        #endregion

        public static bool CheckUser(string login, string password)
        {
            if (true)
            {

                return true;
            }

            return false;
        }

        public static User LoadUser(string login, string password)
        {
            // записываю в пользователя
            setInstance("Jo", login, password);

            // загружаю в базу данных
            DbService.SaveUser(CurrentUser);

            DbService.AddNote(new Note(CurrentUser.UserId, "заметка1", "213", "KGU", false));
            DbService.AddNote(new Note(CurrentUser.UserId, "заметка2", "200", "KGU", false));
            DbService.AddNote(new Note(CurrentUser.UserId, "заметка3", "200", "KGU", false));
            DbService.AddNote(new Note(CurrentUser.UserId, "заметка4", "202", "KGU", false));
            DbService.AddNote(new Note(CurrentUser.UserId, "заметка5", "202", "KGU", false));

            // выгружаю с id
            setInstance(DbService.LoadUserFromDb());

            CurrentUser.Favorites.Add(new Room
            {
                UserId = CurrentUser.UserId,
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
            });
            CurrentUser.Favorites.Add(new Room
            {
                UserId = CurrentUser.UserId,
                Name = "200",
                Floor = 1,
                Details = "The Asian black bear, also known as the moon bear and the white-chested bear, is a medium-sized bear species native to Asia and largely adapted to arboreal life. It lives in the Himalayas, in the northern parts of the Indian subcontinent, Korea, northeastern China, the Russian Far East, the Honshū and Shikoku islands of Japan, and Taiwan. It is classified as vulnerable by the International Union for Conservation of Nature (IUCN), mostly because of deforestation and hunting for its body parts.",
                Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",

            });
            CurrentUser.Favorites.Add(new Room
            {
                UserId = CurrentUser.UserId,
                Name = "202",
                Floor = 2,
                Details = "The brown bear is a bear that is found across much of northern Eurasia and North America. In North America the population of brown bears are often called grizzly bears. It is one of the largest living terrestrial members of the order Carnivora, rivaled in size only by its closest relative, the polar bear, which is much less variable in size and slightly larger on average. The brown bear's principal range includes parts of Russia, Central Asia, China, Canada, the United States, Scandinavia and the Carpathian region, especially Romania, Anatolia and the Caucasus. The brown bear is recognized as a national and state animal in several European countries.",
            });

            DbService.SaveDb();

            return CurrentUser;
        }

        public static void LoginOut()
        {
          //  DbService.RemoveUsersFavorites(CurrentUser.Favorites);
            DbService.RemoveUsersNotes(CurrentUser.Notes);

            DbService.RemoveUser(CurrentUser);

            CurrentUser = null;
        }

        // TODO with server part
        public static void AddNote(Note note)
        {
            // CurrentUser.Notes.Add(note);
            
            DbService.AddNote(note);
            DbService.LoadUserNotes(CurrentUser);
            
            
            // если note ispublic == true -> отправить на сервер
        }

        // TODO with server part
        public static string DeleteNote(int id)
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

        public static Note FindNoteById(int id)
        {
            Note note = null;

            for (int i = 0; i < CurrentUser.Notes.Count; i++)
            {
                if (CurrentUser.Notes[i].Id == id)
                {
                    note = CurrentUser.Notes[i];
                }
            }
            return note;
        }

        // Мысль - обсудить с Ураевой, возможно все заметки хранить на сервере
        // но те, что общие, добавлять именно к аудитории

        public static Room isRoomFavoit(Room room)
        {
            foreach (var fav in CurrentUser.Favorites)
            {
                if (fav.Equals(room))
                {
                    return fav;
                }
            }
            return null;
        }
    }
}