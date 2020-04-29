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

        public static User setInstance(User user)
        {
            CurrentUser = user;
            return CurrentUser;

        }

        public static User setInstance(string name, string login, string pass)
        {
            CurrentUser = new User(name, login, pass);
            return CurrentUser;
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
            DbService.LoadSampleUser("", "");
            return User.CurrentUser;
        }

        public static void LoginOut()
        {
            DbService.RemoveRoom(CurrentUser.Favorites);
            DbService.RemoveNote(CurrentUser.Notes);

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