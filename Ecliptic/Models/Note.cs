using Ecliptic.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecliptic.Models
{
    public class Note : ICloneable
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Date { get; set; }

        public string Room { get; set; }

        public string Building { get; set; }

        public bool isPublic { get; set; } // общая ли заметтка или нет


        public virtual int? UserId { get; set; }
        public virtual User User { get; set; }

        public Note()
        {

        }

        // добавление заметок пользователя
        public Note(int userid, string text, string room, string building, bool acsess)
        {
            Text = text;

            Date = DateTime.Today.ToString();
            Room = room;
            Building = building;
            isPublic = acsess;
            UserId = userid;
        }

        // добавление общих заметок в NoteData
        public Note(string text, string room, string building, bool acsess)
        {
            Text = text;

            Date = DateTime.Today.ToString();
            Room = room;
            Building = building;
            isPublic = acsess;
        }

        public object Clone()
        {
            return new Note
            {
                Text = this.Text,
                Date = this.Date,
                Room = this.Room,
                Building = this.Building,
                isPublic = this.isPublic,
            };
        }

        public override bool Equals(object obj)
        {
            return obj is Note note  &&
                   Text == note.Text &&
                   Date == note.Date &&
                   Room == note.Room &&
                   Building == note.Building &&
                   isPublic == note.isPublic;
        }
    }
}