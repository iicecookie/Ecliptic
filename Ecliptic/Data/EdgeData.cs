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
        // все
        public static List<EdgeM> Edges { get; set; }

        // для стен
        public static List<EdgeM> CurrentFloorWalls { get; set; }

        // для маршрутов
        public static List<EdgeM> Ways { get; set; }

        static EdgeData()
        {
            Edges = new List<EdgeM>();
            CurrentFloorWalls = new List<EdgeM>();
            Ways = new List<EdgeM>();
        }

        static public void ConvertPathToWay(List<PointM> path)
        {
            Ways = new List<EdgeM>();
            for (int p = 0; p < path.Count-1; p++)
            {
                List<EdgeM> edges = new List<EdgeM>();
                edges.AddRange(path[p].EdgesIn);
                edges.AddRange(path[p].EdgesOut);

                foreach (var e in edges)
                {
                    if (e.isThatEdge(path[p], path[p + 1]))
                    {
                        Ways.Add(e);
                        break;
                    }
                }
            }
        }
    }
}