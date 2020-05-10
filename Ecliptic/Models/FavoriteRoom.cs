using System;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class FavoriteRoom
    {
        public int FavoriteRoomId { get; set; }

        public string Name     { get; set; }
        public string Building { get; set; }
        public string Details  { get; set; }

        public virtual User User   { get; set; }
        public virtual int? UserId { get; set; }


        public FavoriteRoom()
        {

        }

        public FavoriteRoom(string name, string details, string building = "",
                            int userid = 0, int roomId = 0)
        {
            Name = name;
            Building = building;

            Details = details;

            UserId = userid;

            FavoriteRoomId = roomId;
        }
    }
}