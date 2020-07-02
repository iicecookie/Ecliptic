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
        public static List<EdgeM> Edges { get; set; } // все ребра здания

        public static List<EdgeM> CurrentFloorWalls { get; set; } // ребра стен текущего открытого этажа

        public static List<EdgeM> Ways { get; set; } // список маршрутов здания

        static EdgeData()
        {
            Edges = new List<EdgeM>();
            CurrentFloorWalls = new List<EdgeM>();
            Ways = new List<EdgeM>();
        }

       /// <summary>
       /// Преобразование последовательности точек маршрута в последовательность ребер 
       /// для отображения
       /// и ее запись в текущий маршрут Ways
       /// </summary>
       /// <param name="path">Последовательность маршрутных точек в маршруте</param>
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