using Ecliptic.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecliptic.Models
{
    [Table("Notes")]
    public class Note
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        public string Text { get; set; }

        public string Date { get; private set; }

        public string Room { get; private set; }

        public string Building { get; private set; }

        public bool isPublic { get; set; } // общая ли заметтка или нет


   //     [DisplayName("Владелец карты")]
   //     public virtual User Owner { get; set; }

        public Note()
        {

        }
        // добавление заметок пользователя
        public Note(User user, string text, string room, string building, bool acsess)
        {
            Id = User.CurrentUser.Notes.Count;
            Text = text;
            Date = new DateTime().ToString();
            Room = room;
            Building = building;
            isPublic = acsess;
        //    Owner = user;
        }

        // добавление общих заметок в NoteData
        public Note(string text, string room, string building, bool acsess)
        {
            Id = NoteData.Notes.Count;
            Text = text;
            Date = new DateTime().ToString();
            Room = room;
            Building = building;
            isPublic = acsess;
     //       Owner = User.CurrentUser;
        }
    }
}