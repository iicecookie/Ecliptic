using System;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class Room : ICloneable
    {
        public int RoomId  { get; set; }

        public string Name { get;  set; }

        public string Description { get;  set; } // описание

        public string Timetable { get; set; }   // расписание
        public string Phone     { get; set; }
        public string Site      { get; set; }

        public virtual int?  FloorId { get; set; }
        public virtual Floor Floor   { get; set; }

        public virtual List<Worker> Workers { get; set; } // ответственные лица 
        public virtual List<Note>   Notes   { get; set; } // публичные заметки о этом помещении 

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

        // Преобразовать помещение в избранное для пользователся
        public FavoriteRoom ToFavRoom(int Clientid)
        {
            return new FavoriteRoom(Name, Description, Floor.Building.Name, Clientid, RoomId);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
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