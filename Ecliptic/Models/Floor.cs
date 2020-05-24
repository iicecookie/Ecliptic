using Ecliptic.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecliptic.Models
{
	public class Floor
	{
		public int FloorId { get;  set; }

		public int Level   { get;  set; }

		public Building Building   { get;  set; }
		public int?     BuildingId { get;  set; }

		public List<Room> Rooms { get;  set; }

		public List<PointM> Points { get; set; }


		public Floor()
		{
			Rooms  = new List<Room>();
			Points = new List<PointM>();
		}

		public Floor(int level, int floorid = 0, int buildingid = 0) : this()
		{
			FloorId = floorid;

			this.Level = level;

			this.BuildingId = buildingid;
		}
	}
}