using System;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class Room : ICloneable
    {
        public int RoomId  { get; set; }

        public string Name { get; private set; } 

        public int    Floor       { get; private set; } // этаж СТАНЕТ ССЫЛКОЙ НА ЭТАЖ
        public string Details     { get; private set; } // ну хз зачем дважды
        public string Description { get; private set; } // описание

        public string Timetable   { get; private set; } // расписание
        public string Phone       { get; private set; } 
        public string Site        { get; private set; } 

        public virtual List<Worker> Workers { get; set; } // работники 
        public virtual List<Note>   Notes   { get; set; } // публичные заметки 

        public Room()
        {
            Workers = new List<Worker>();
            Notes = new List<Note>();
        }

        public Room(string name, int floor,
                    int    roomid = 0,
                    string details   = null, string description = null,
                    string timetable = null, 
                    string phone = null, string site = null) : this()
        {
            RoomId = roomid;

            Name = name;
            Floor = floor;
            Details = details;
            Description = description;

            Timetable = timetable;
            Phone = phone;
            Site = site;
        }

        public FavoriteRoom ToFavRoom(int Userid)
        {
            return new FavoriteRoom(Name, Details, "", Userid);
        }

        public object Clone()
        {
            return new Room
            {
                Name = this.Name,
                Floor = this.Floor,
                Details = this.Details,
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
                   Details == room.Details &&
                   Description == room.Description &&
                   Timetable == room.Timetable &&
                   Phone == room.Phone &&
                   Site == room.Site;
        }
    }
}