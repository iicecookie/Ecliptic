using Ecliptic.Data;
using Ecliptic.Repository;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class User
    {
        #region Params

        public readonly int    UserId;
        public readonly string Name;
        public readonly string Login;
        public readonly string Password;

        public virtual List<Note> Notes     { get; set; }

        public virtual List<FavRoom> Favorites { get; set; }

        public static User CurrentUser { get; private set; }
        #endregion

        #region Constructors

        private User()
        {
            Notes     = new List<Note>();
            Favorites = new List<FavRoom>();
        }

        private User(int id, string Name, string login, string pass) : this()
        {
            this.UserId = id;
            this.Name = Name;
            this.Login = login;
            this.Password = pass;
        }

        public static User setUser(User user)
        {
            CurrentUser = user;
            return CurrentUser;

        }

        public static User setUser(int id, string name, string login, string pass)
        {
            CurrentUser = new User(id, name, login, pass);
            return CurrentUser;
        }

        #endregion

        public static User LoadUser(string login, string password)
        {
            DbService.LoadSampleUser("", "");
            return User.CurrentUser;
        }

        public static void LoginOut()
        {
            DbService.RemoveFavRoom(CurrentUser.Favorites);
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

        public static FavRoom isRoomFavoit(Room room)
        {   
            foreach (var fav in CurrentUser.Favorites)
            {
                if (fav.Name == room.Name && fav.Details == room.Details)
                {
                    return fav;
                }
            }
            return null;
        }
    }
}