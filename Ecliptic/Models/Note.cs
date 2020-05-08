using Ecliptic.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecliptic.Models
{
    public class Note : ICloneable
    {
        public int NoteId { get; set; }

        public string Text { get; set; }
        public string Date { get; set; }
        public bool   isPublic { get; set; } // общая ли заметтка или нет

        public string Building { get; set; } // может не нужно

        public virtual int? RoomId { get; set; }
        public virtual Room Room { get; set; }

        public virtual int? UserId { get; set; }
        public virtual User User { get; set; }

      //  public Note() { }

        public Note(string text, string building, bool acsess,
                    int? roomid = null, int? userid = null, int noteid = 0)
        {
            NoteId   = noteid;

            Text     = text;
            isPublic = acsess;
            Date = DateTime.Today.ToString();

            RoomId = roomid;
            UserId = userid;

            Building = building;
        }

        public object Clone()
        {
            return new Note(Text, Building, isPublic, RoomId);
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