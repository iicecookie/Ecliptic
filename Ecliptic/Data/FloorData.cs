using Ecliptic.Data;
using System;
using System.Linq;
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
 
            Floors = Floors.OrderBy(f => f.Level).ToList();
        }

        public static Floor GetFloor(int? level)
        {
            foreach (var floor in Floors)
            {
                if (floor.Level == level)
                {
                    return floor;
                }
            }
            return null;
        }

        public static int GetMaxLevel()
        {
            if (Floors.Count == 0) return 0;
            return Floors.Max(s => s.Level);
        }

        public static int GetMinLevel()
        {
            if (Floors.Count == 0) return 0;
            return Floors.Min(s => s.Level);
        }

    }
}