using Ecliptic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic.Models
{
    public class User
    {
        public static User CurrentUser;

        public string Name
        {
            get;// private 
            set;
        }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public List<Note> Notes { get; set; }

        public static string DeleteNote(string id)
        {
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




        public static bool isNull
        {
            get
            {
                if (CurrentUser == null)
                    return true;
                return false;
            }
        }

        private User()
        {
            Notes = new List<Note>();
        }
        private User(string Name) : this()
        {
            this.Name = Name;
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

            User.CurrentUser.Notes.Add(new Note("1", "заметка1", "409"));
            User.CurrentUser.Notes.Add(new Note("2", "заметка2", "213"));
            User.CurrentUser.Notes.Add(new Note("3", "заметка3", "200"));
            User.CurrentUser.Notes.Add(new Note("4", "заметка4", "200"));
            User.CurrentUser.Notes.Add(new Note("5", "заметка5", "202"));
            User.CurrentUser.Notes.Add(new Note("6", "заметка6", "202"));

            // RoomData.AddNotes();
        }
    }
}