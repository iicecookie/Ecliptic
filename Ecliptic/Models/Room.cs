﻿using System;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class Room : ICloneable
    {
        public int RoomId { get; set; }

        public string Name { get; set; } // Имя аудитории +
        public int    Floor { get; set; } // этаж
        public string Details { get; set; } //
        public string Description { get; set; } // описание
        public string Timetable { get; set; } // расписание
        public string Phone { get; set; } // телефон
        public string Site { get; set; } // сайт

        public virtual List<Worker> Workers { get; set; } // работники 

        public virtual int? UserId { get; set; }
        public virtual User User { get; set; }

        public Room()
        {
            Description = " ";
            Workers = new List<Worker>();
        }

        public Room(string name, int floor, 
                    string details=null, string description=null, string timetable=null, 
                    string phone=null,   string site=null,  List<Worker> workers = null,
                    User user = null) : this()
        {
           // RoomId = roomId;
            Name = name;
            Floor = floor;
            Details = details;
            Description = description;
            Timetable = timetable;
            Phone = phone;
            Site = site;

            if (workers != null)
                Workers = workers;

            if (user != null)
                UserId = user.UserId;
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