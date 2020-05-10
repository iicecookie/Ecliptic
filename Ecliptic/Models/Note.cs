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
        public bool   isPublic { get; set; }

        public string Building { get; set; } // нужна для прототы выборки заметок по зданию

        public virtual int? RoomId { get; set; }
        public virtual Room Room   { get; set; }

        public virtual int? UserId { get; set; }
        public virtual User User   { get; set; }
        public string  UserName    { get; set; } // для публичных заметок, что бы знать чья она

        public Note() { }   

        public Note(string text, string building, bool acsess,
                    int noteid = 0,     int? roomid = null, 
                    int? userid = null, string username = "")
        {
            NoteId   = noteid;

            Text     = text;
            Date     = DateTime.Today.ToString();
            isPublic = acsess;

            RoomId = roomid;
            UserId = userid;

            Building = building;
            UserName = username;
        }

        // посмотри, что для чего это вообще
        public object Clone()
        {
            return new Note(Text, Building, isPublic, roomid: RoomId);
        }

        public override bool Equals(object obj)
        {
            return obj is Note note  &&
                   Text == note.Text &&
                   Date == note.Date &&
                   RoomId == note.RoomId &&
                   Building == note.Building &&
                   isPublic == note.isPublic;
        }
    }
}