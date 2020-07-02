using System.Collections.Generic;

namespace Ecliptic.Models
{
	// точки связывающие стены здания и ребра в маршруты
	public class PointM
	{
		public int Id { get; set; }

		public double X { get; set; }
		public double Y { get; set; } 

		public bool  IsWaypoint { get; set; } // является ли точка маршрутной

		public int?  FloorId { get; set; }
		public Floor Floor   { get; set; }

		public int?  RoomId  { get; set; }
		public Room  Room    { get; set; }

		public virtual List<EdgeM> EdgesIn  { get; set; } // связаные ребра
		public virtual List<EdgeM> EdgesOut { get; set; } // списка 2 потому что иначе проблема с выгрузкой данных с базы
		// нет разницы между первым и вторым списком т.к. граф неориентированый

		public PointM()
		{
			EdgesIn  = new List<EdgeM>();
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
