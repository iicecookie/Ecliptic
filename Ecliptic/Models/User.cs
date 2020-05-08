using Ecliptic.Data;
using Ecliptic.Repository;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class User
    {
        #region Params

        public static User CurrentUser { get; private set; }

        public int UserId { get; set; }

        public readonly string Name;
        public readonly string Login;

        public virtual List<Note> Notes     { get; set; }

        public virtual List<FavoriteRoom> Favorites { get; set; }

        #endregion

        #region Constructors

        private User()
        {
            Notes     = new List<Note>();
            Favorites = new List<FavoriteRoom>();
        }

        private User(int id, string Name, string login) : this()
        {
            this.UserId = id;
            this.Name = Name;
            this.Login = login;
        }

        public static User setUser(User user)
        {
            CurrentUser = user;
            return CurrentUser;
        }

        public static User setUser(int id, string name, string login)
        {
            CurrentUser = new User(id, name, login);
            return CurrentUser;
        }

        #endregion

        // sample
        public static User LoadUser(string login, string password)
        {
            DbService.LoadSampleUser("", "");
            return User.CurrentUser;
        }

        public static void LoginOut()
        {
            DbService.RemoveFavoriteRoom(CurrentUser.Favorites);
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
                if (CurrentUser.Notes[i].NoteId == id)
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
                if (CurrentUser.Notes[i].NoteId == id)
                {
                    note = CurrentUser.Notes[i];
                }
            }
            return note;
        }

        // Мысль - обсудить с Ураевой, возможно все заметки хранить на сервере
        // но те, что общие, добавлять именно к аудитории

        public static FavoriteRoom isRoomFavoit(Room room)
        {   
            foreach (var favorite in CurrentUser.Favorites)
            {
                if (favorite.Name == room.Name && 
                    favorite.Details == room.Details || 
                    favorite.FavoriteRoomId == room.RoomId)
                {
                    return favorite;
                }
            }
            return null;
        }
    }
}