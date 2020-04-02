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

            CurrentUser.Favorites.Add(RoomData.Roooms[0]);
            CurrentUser.Favorites.Add(RoomData.Roooms[1]);
            CurrentUser.Favorites.Add(RoomData.Roooms[2]);
            CurrentUser.Favorites.Add(RoomData.Roooms[3]);
            CurrentUser.Favorites.Add(RoomData.Roooms[4]);

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