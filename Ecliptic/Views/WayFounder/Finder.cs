using Ecliptic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecliptic.Views.WayFounder
{
    public static class Way
    {
        public static Room Begin { get; set; }
        public static Room End { get; set; }
    }

    // Информация о вершине
    public class GraphVertexInfo
    {
        public PointM Vertex { get; set; } // Вершина

        public bool IsUnvisited { get; set; } // Не посещенная вершина

        public double EdgesWeightSum { get; set; } // Сумма весов ребер

        public PointM PreviousVertex { get; set; } // Предыдущая вершина

        public GraphVertexInfo(PointM vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = double.MaxValue;
            PreviousVertex = null;
        }
    }

    public class Dijkstra
    {
        List<GraphVertexInfo> infos;

        // Инициализация информации
        void InitInfo()
        {
            infos = new List<GraphVertexInfo>();

            List<PointM> vertexs = PointData.Points
                                         .Where(p => p.IsWaypoint == true)
                                         .ToList();

            foreach (var v in vertexs)
            {
                infos.Add(new GraphVertexInfo(v));
            }
        }

        GraphVertexInfo GetVertexInfo(PointM v)
        {
            foreach (var i in infos)
            {
                if (i.Vertex.Equals(v))
                {
                    return i;
                }
            }
            return null;
        }

        // Поиск непосещенной вершины с минимальным значением суммы
        public GraphVertexInfo FindUnvisitedVertexWithMinSum()
        {
            double minValue = double.MaxValue;
            GraphVertexInfo minVertexInfo = null;
            foreach (GraphVertexInfo i in infos)
            {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                {
                    minVertexInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }

            return minVertexInfo;
        }

        // Поиск кратчайшего пути по названиям вершин
        public List<PointM> FindShortestPath(int startName, int finishName)
        {
            return FindShortestPath(PointData.Find(startName), PointData.Find(finishName));
        }

        // Поиск кратчайшего пути по вершинам
        public List<PointM> FindShortestPath(PointM startVertex, PointM finishVertex)
        {
            InitInfo();
            var first = GetVertexInfo(startVertex);
            first.EdgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null) { break; }

                SetSumToNextVertex(current);
            }

            return GetPath(startVertex, finishVertex);
        }

        // Вычисление суммы весов ребер для следующей вершины
        void SetSumToNextVertex(GraphVertexInfo info)
        {
            info.IsUnvisited = false;
            List<EdgeM> edges = new List<EdgeM>();
            edges.AddRange(info.Vertex.EdgesIn);
            edges.AddRange(info.Vertex.EdgesOut);

            foreach (var e in edges)
            {
                var nextInfo = GetVertexInfo(e.PointTo); //erwrwerwer
                var sum = info.EdgesWeightSum + e.Weight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }

                nextInfo = GetVertexInfo(e.PointFrom); //erwrwerwer
                sum = info.EdgesWeightSum + e.Weight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }

        // Формирование пути
        List<PointM> GetPath(PointM startVertex, PointM endVertex)
        {
            List<PointM> path = new List<PointM>();
            path.Add(endVertex);

            while (startVertex != endVertex)
            {
                endVertex = GetVertexInfo(endVertex).PreviousVertex;
                path.Add(endVertex);
            }
            return path;
        }
    }
}
