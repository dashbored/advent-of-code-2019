using System;
using System.Collections.Generic;
using System.Text;

namespace Day11
{
	public class Coordinate
	{
		// 0 = black, 1 = white
		public int Color { get; internal set; }

		public int X { get; set; }
		public int Y { get; set; }
		public List<int> previousColors = new List<int>();

		public Coordinate(int x, int y)
		{
			X = x;
			Y = y;
		}

		public void SetColor(int color)
		{
			previousColors.Add(Color);
			Color = color;
		}
	}
}
