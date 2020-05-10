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

        public static bool isThatRoom(Room room)
        {
            foreach (var i in Rooms)
            {
                if (i.Equals(room))
                    return true;
            }
            return false;
        }

        public static bool isThatRoom(FavoriteRoom favorite)
        {
            foreach (var room in Rooms)
            {
                if (room.Name == favorite.Name &&
                    room.Description == favorite.Details ||
                    room.RoomId == favorite.FavoriteRoomId)
                    return true;
            }
            return false;
        }

        public static Room isThatRoom(string room)
        {
            foreach (var i in Rooms)
            {
                if (i.Name == room)
                    return i;
            }
            return null;
        }
    }
}