using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecliptic.Models;
using Ecliptic.Repository;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace Ecliptic.Data
{
    public static class RoomData
    {
        public static List<Room> Rooms { get; set; }

        static RoomData()
        {
            Rooms = new List<Room>();
        }

        public static bool isThatRoom(string room)
        {
            foreach (var i in Rooms)
            {
                if (i.Name == room)
                    return true;
            }
            return false;
        }
    }
}