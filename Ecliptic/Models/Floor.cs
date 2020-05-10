using Ecliptic.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecliptic.Models
{
	public class Floor
	{
		public int FloorId { get; private set; }

		public int Level   { get; private set; }

		public Building Building   { get; private set; }
		public int?     BuildingId { get; private set; }

		public List<Room> Rooms { get; private set; }

		// public List<Point> Points { get; set; }


		public Floor()
		{
			Rooms = new List<Room>();
			// Points = new List<Point>();
		}

		public Floor(int level, int floorid = 0, int buildingid = 0) : this()
		{
			FloorId = floorid;

			this.Level = level;

			this.BuildingId = buildingid;
		}
	}
}