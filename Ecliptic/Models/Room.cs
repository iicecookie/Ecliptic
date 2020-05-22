using System;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class Room : ICloneable
    {
        public int RoomId  { get; set; }

        public string Name { get; private set; }

        public string Description { get; private set; } // описание

        public string Timetable { get; private set; } // расписание
        public string Phone     { get; private set; }
        public string Site      { get; private set; }

        public virtual int?  FloorId { get; set; }
        public virtual Floor Floor   { get; set; }

        public virtual List<Worker> Workers { get; set; } // работники 
        public virtual List<Note> Notes     { get; set; } // публичные заметки 

        public Room()
        {
            Workers = new List<Worker>();
            Notes = new List<Note>();
        }

        public Room(string name, string description = null, 
                    string timetable = null,
                    string phone = null, string site = null,
                    int floorid = 0, int roomid = 0) : this()
        {
            RoomId = roomid;

            Name = name;
            FloorId = floorid;
            Description = description;

            Timetable = timetable;
            Phone = phone;
            Site = site;
        }

        // ПРОВЕРИТЬ ЭТУ ЧАСТИ
        public FavoriteRoom ToFavRoom(int Clientid)
        {
            return new FavoriteRoom(Name, Description, "KSU", Clientid, RoomId);
        }

        public object Clone()
        {
            return new Room
            {
                Name = this.Name,
                Floor = this.Floor,
                Description = this.Description,
                Timetable = this.Timetable,
                Phone = this.Phone,
                Site = this.Site,
                Workers = this.Workers,
            };
        }

        public override bool Equals(object obj)
        {
            return obj is Room room &&
                   Name == room.Name &&
                   Floor == room.Floor &&
                   Description == room.Description &&
                   Timetable == room.Timetable &&
                   Phone == room.Phone &&
                   Site == room.Site;
        }
    }
}