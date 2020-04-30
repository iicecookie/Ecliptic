using Ecliptic.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecliptic.Models
{
    public static class FloorData
    {
        public static List<Floor> Floors { get; set; }

        static FloorData()
        {
            Floors = new List<Floor>();

            FloorData.Floors.Add(new Floor() { Level = -1, });
            FloorData.Floors.Add(new Floor() { Level = 1, });
            FloorData.Floors.Add(new Floor() { Level = 2, });
            FloorData.Floors.Add(new Floor() { Level = 3, });
        }

        public static Floor FindNote(Floor floor)
        {
            foreach (var i in Floors)
            {
                if (i.Equals(floor))
                {
                    return i;
                }
            }
            return null;
        }
    }
}