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
        public static List<Room> Rooms { get; set; } // помещения здания

        static RoomData()
        {
            Rooms = new List<Room>();
        }

        /// <summary>
        /// Проверка, есть ли избранное помещение среди загруженных
        /// </summary>
        /// <param name="favorite">избранное помещение (со страницы избранных пользователя)</param>
        /// <returns>статус избранности</returns>
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

        /// <summary>
        /// Поиск помещения по его названию
        /// </summary>
        /// <param name="room">имя помещения</param>
        /// <returns>ссылка на помещение</returns>
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