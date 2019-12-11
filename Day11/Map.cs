using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day11
{
	class Map
	{
		private List<Coordinate> Grid { get; set; }

		public Map()
		{
			Grid = new List<Coordinate>();
			var startingPosition = new Coordinate(0, 0);
			Grid.Add(startingPosition);
		}

		internal Coordinate GetStartingPosition()
		{
			return Grid.First();
		}

		internal Coordinate GetCoordinate(int x, int y)
		{
			var coordinate = Grid.SingleOrDefault(c => c.X == x && c.Y == y);
			if (coordinate == null)
			{
				return new Coordinate(x, y);
			}
			return coordinate;
		}

		public int GetNumberOfPaintedCoordinates()
		{
			return Grid.Select(c => c.previousColors.Count > 0).Count();
		}

		public List<Coordinate> GetGrid()
		{
			return Grid;
		}
	}
}
