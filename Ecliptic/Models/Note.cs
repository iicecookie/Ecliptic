using Ecliptic.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecliptic.Models
{
    public class Note // : ICloneable
    {
        public int NoteId { get; set; }

        public string Text { get; set; }
        public string Date { get; set; }
        public bool   isPublic { get; set; }

        public string RoomName { get; set; } // что бы знать если не загружено здание
        public string Building { get; set; } // нужна для прототы выборки заметок по зданию
        public string UserName { get; set; } // для публичных заметок, что бы знать чья она

        public virtual int? RoomId { get; set; }
        public virtual Room Room   { get; set; }

        public virtual int? UserId { get; set; }
        public virtual User User   { get; set; }

        public Note() { }   

        public Note(string text,        string building, 
                    string roomname,    bool acsess,
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
            RoomName = roomname;
            UserName = username;
        }

        // посмотри, что для чего это вообще
        // я думал делать новую заметку для публичных, но это бред какой- то 
        // public object Clone()
        // {
        //     return new Note(Text, Building, isPublic, roomid: RoomId);
        // }

        public override bool Equals(object obj)
        {
            return obj is Note note &&
                   Text == note.Text &&
                   RoomId == note.RoomId &&
                   UserName == note.UserName && // новые
                   UserId == note.UserId &&     // новые
                   Building == note.Building;
        }
    }
}