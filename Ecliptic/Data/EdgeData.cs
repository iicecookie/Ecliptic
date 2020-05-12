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

        public static List<EdgeM> CurrentFloorEdges { get; set; }

        static EdgeData()
        {
            Edges = new List<EdgeM>();
            CurrentFloorEdges = new List<EdgeM>();
        }
    }
}