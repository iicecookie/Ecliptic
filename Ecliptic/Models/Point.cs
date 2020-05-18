using System.Collections.Generic;

namespace Ecliptic.Models
{
	public class PointM
	{
		public int Id { get; set; }

		public double X { get; private set; }
		public double Y { get; private set; }

		public bool  IsWaypoint { get; private set; }

		public int?  FloorId { get; set; }
		public Floor Floor   { get; set; }

		public int?  RoomId  { get; set; }
		public Room  Room    { get; set; }

		public virtual List<EdgeM> EdgesIn  { get; set; }
		public virtual List<EdgeM> EdgesOut { get; set; }

		public PointM()
		{
			EdgesIn = new List<EdgeM>();
			EdgesOut = new List<EdgeM>();
		}

		public PointM(double x, double y,
					  bool isWaypoint = false, int pointId = 0,
					  int? floorId = null, int? roomId = null) : this()
		{
			Id = pointId;
			X = x;
			Y = y;
			IsWaypoint = isWaypoint;
			FloorId = floorId;
			RoomId = roomId;
		}

		public PointM(PointM point) : this()
		{
			Id = point.Id;
			X = point.X;
			Y = point.Y;
			IsWaypoint = point.IsWaypoint;
			Floor = point.Floor;
			FloorId = point.FloorId;
			Room = point.Room;
			RoomId = point.RoomId;
			EdgesIn = point.EdgesIn;
			EdgesOut = point.EdgesOut;
		}

		public override bool Equals(object obj)
		{
			return obj is PointM m &&
				   Id == m.Id &&
				   IsWaypoint == m.IsWaypoint &&
				   FloorId == m.FloorId;
		}

		public override string ToString()
		{
			return Id + " ";
		}
	}
}
