using System;
using System.Collections.Generic;
using System.Text;

namespace Day10
{
	public struct Coordinate
	{
		public int X { get; set; }
		public int Y { get; set; }
		public bool HasAsteroid { get; set; }

		public Coordinate(int x, int y, bool hasAsteroid)
		{
			X = x;
			Y = y;
			HasAsteroid = hasAsteroid;
		}
	}
}
