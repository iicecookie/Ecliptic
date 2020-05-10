using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Ecliptic.Models
{
    public class Building
    {
        public int    BuildingId  { get; set; }

        public string Name        { get; private set; }
        public string Description { get; private set; }

        public string Addrees     { get; private set; }
        public string TimeTable   { get; private set; }
        public string Email       { get; private set; }

        public List<Floor> Floors { get; set; }

        public Building()
        {
            Floors = new List<Floor>();
        }

        public Building(string name, string description = null,
                        string addrees = null, string timetable = null,
                        string email = null, int buildingid = 0) : this()
        {
            BuildingId = buildingid;

            Name = name;
            Description = description;
            
            Addrees   = addrees;
            TimeTable = timetable;
            Email     = email;
        }
    }
}
