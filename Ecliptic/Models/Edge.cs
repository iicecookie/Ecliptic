namespace Ecliptic.Models
{
	// ребро, оно же стена, оно же маршрут.
	public class EdgeM
	{
		public int Id { get; set; }

		public double Weight { get; set; } // длинна

		public int? PointFromId { get; set; }
		public virtual PointM PointFrom { get; set; } // точка начала

		public int? PointToId { get; set; }
		public virtual PointM PointTo { get; set; } // точка конца (направленность не имеет значения)

		public EdgeM() { }

		public EdgeM(double weight, int? pointFirId = null, int? pointSecId = null, int edgeId = 0)
		{
			Id = edgeId;
			Weight = weight;
			PointFromId = pointFirId;
			PointToId = pointSecId;
		}

		/// <summary>
		/// Проверка,  это ли ребро связывает точки (при преобразовании точек в маршрут)
		/// </summary>
		public bool isThatEdge(PointM first, PointM second)
		{
			if (PointFrom == first && PointTo == second)
				return true;
			if (PointFrom == second && PointTo == first)
				return true;
			return false;
		}
	}
}
