using Ecliptic.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Android.Graphics;

namespace Ecliptic.Models
{
    public static class PointData
    {
        public static List<PointM> Points     { get; set; }

        public static List<PointM> RoomPoints { get; set; }

        static PointData()
        {
            Points = new List<PointM>();
            RoomPoints = new List<PointM>();
        }

        static public PointM Find(int id)
        {
            foreach(var i in Points)
            {
                if (i.Id == id)
                {
                    return i;
                }
            }
            return null;
        }

        static public PointM Find(Room room)
        {
            foreach (var i in RoomPoints)
            {
                if (i.Room.RoomId == room.RoomId)
                {
                    return i;
                }
            }
            return null;
        }
    }
}