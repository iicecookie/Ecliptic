namespace Ecliptic.Models
{
	public class EdgeM
	{
		public int Id { get; set; }

		public double  Weight   { get; set; }

		public int?    PointFromId      { get; set; }
		public virtual PointM PointFrom { get; set; }

		public int?    PointToId        { get; set; }
		public virtual PointM PointTo   { get; set; }

		public EdgeM() { }

		public EdgeM(double weight, int? pointFirId = null, int? pointSecId = null, int edgeId = 0)
		{
			Id = edgeId;
			Weight = weight;
			PointFromId = pointFirId;
			PointToId   = pointSecId;
		}

		public bool isThatEdge(PointM first,PointM second)
		{
			if (PointFrom == first && PointTo == second)
				return true;
			if (PointFrom == second && PointTo == first)
				return true;
			return false;
		}
	}
}
