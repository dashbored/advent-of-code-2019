using System;
using System.Collections.Generic;
using System.Text;

namespace Day10
{
	class Map
	{
		public List<Coordinate> Coordinates { get; set; }
		public Map(char[] definition)
		{
			Coordinates = new List<Coordinate>();
			Initialize(definition);
		}

		private void Initialize(char[] definition)
		{
			int row = 0, col = 0;
			foreach (var c in definition)
			{
				if (c == '\n')
				{
					row++;
					continue;
				}
				var coord = new Coordinate(row, col, c == '#' ? true : false);
				col++;
				Coordinates.Add(coord);
			}
		}
	}
}
