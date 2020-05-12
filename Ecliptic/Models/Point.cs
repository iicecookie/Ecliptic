using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Xamarin.Forms;

namespace Ecliptic.Models
{
    public class PointM
    {
		public int Id { get; set; }

		public double X { get; private set; }
		public double Y { get; private set; }

		public bool IsWaypoint { get; private set; }

		public int?  FloorId { get; set; }
		public Floor Floor   { get; set; }

		public int?  RoomId  { get; set; }
		public Room  Room    { get; set; }

		 public virtual List<EdgeM> EdgesIn  { get; set; }
		 public virtual List<EdgeM> EdgesOut { get; set; }

		public PointM()
		{
			EdgesIn  = new List<EdgeM>();
			EdgesOut = new List<EdgeM>();
		}

		public PointM(double x, double y, 
					  bool isWaypoint = false, int pointId = 0, 
					  int? floorId    = null,  int? roomId = null)
		{
			Id = pointId;
			X = x;
			Y = y;
			IsWaypoint = isWaypoint;
			FloorId = floorId;
			RoomId = roomId;
		}
	}
}
