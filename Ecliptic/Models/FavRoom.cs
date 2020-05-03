using System;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class FavRoom
    {
        public int FavRoomId { get; set; }

        public string Name { get; set; } 
     
        public string Details { get; set; }

        public virtual User User { get; set; }
        public virtual int? UserId { get; set; }

        public FavRoom(string name, string details, int userid, int roomId = 0)
        {
            FavRoomId = roomId;
            Name = name;
            Details = details;

            UserId = userid;
        }
    }
}