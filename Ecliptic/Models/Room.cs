//using SQLite;
using System;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class Room : ICloneable
    {
    //    [PrimaryKey, AutoIncrement, Column("id")]
        public int RoomId { get; set; }

        public string Name { get; set; } // Имя аудитории +
        public int    Floor { get; set; } // этаж
        public string Details { get; set; } //
        public string Description { get; set; } // описание
        public string Timetable { get; set; } // расписание
        public string Phone { get; set; } // телефон
        public string Site { get; set; } // сайт

        public virtual List<Worker> Workers { get; set; } // работники 

        public Room()
        {
            Description = " ";
            Workers = new List<Worker>();
        }

        public Room(int roomId, string name, int floor, string details, string description, string timetable, string phone, string site, List<Worker> workers = null) : this()
        {
            RoomId = roomId;
            Name = name;
            Floor = floor;
            Details = details;
            Description = description;
            Timetable = timetable;
            Phone = phone;
            Site = site;
            if (workers != null)
                Workers = workers;
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

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(RoomId);
            hash.Add(Name);
            hash.Add(Floor);
            hash.Add(Details);
            hash.Add(Description);
            hash.Add(Timetable);
            hash.Add(Phone);
            hash.Add(Site);
            hash.Add(Workers);
            return hash.ToHashCode();
        }
    }
}