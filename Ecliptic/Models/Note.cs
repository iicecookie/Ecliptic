using Ecliptic.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Ecliptic.Models
{
    public class Note : ICloneable
    {
        public int NoteId { get; set; }

        public string Text { get; set; }
        public string Date { get; set; }
        public bool  isPublic { get; set; }

        public string RoomName   { get; set; } // что бы знать если не загружено здание
        public string Building   { get; set; } // нужна для прототы выборки заметок по зданию
        public string ClientName { get; set; } // для публичных заметок, что бы знать чья она

        public virtual int? RoomId { get; set; }
        public virtual Room Room   { get; set; }

        public virtual int?   ClientId { get; set; }
        public virtual Client Client   { get; set; }

        public Note() { }   

        public Note(string text,          string roomname, 
                    string building = "", bool acsess = false,
                    int noteid = 0,       int? roomid = null, 
                    int? clientid = null, string clientname = "")
        {
            NoteId   = noteid;

            Text     = text;

            Date = DateTime.Now.ToString("dd/MM/yyyy");

            isPublic = acsess;

            RoomId   = roomid;
            ClientId = clientid;

            Building = building;
            RoomName = roomname;
            ClientName = clientname;
        }

        public override bool Equals(object obj)
        {
            return obj is Note note &&
                   Text == note.Text &&
                   RoomId == note.RoomId &&
                   ClientName == note.ClientName && // новые
                   ClientId == note.ClientId &&     // новые
                   Building == note.Building;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}