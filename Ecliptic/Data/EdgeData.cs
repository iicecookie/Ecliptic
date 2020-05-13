using Ecliptic.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecliptic.Models
{
    public static class EdgeData
    {
        public static List<EdgeM> Edges { get; set; }

        // для стен
        public static List<EdgeM> CurrentFloorWalls { get; set; }

        // для маршрутов
        public static List<EdgeM> CurrentFloorWays { get; set; }

        static EdgeData()
        {
            Edges = new List<EdgeM>();
            CurrentFloorWalls = new List<EdgeM>();
            CurrentFloorWays = new List<EdgeM>();
        }
    }
}