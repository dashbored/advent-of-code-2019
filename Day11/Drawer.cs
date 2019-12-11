using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day11
{
	public class Drawer
	{
		const int GRID_CENTER = 5;
		public void Draw(List<Coordinate> grid, Coordinate position, Directions direction)
		{
			
			var maxX = grid.Max(c => c.X);
			var maxY = grid.Max(c => c.Y);
			var minX = grid.Min(c => c.X);
			var minY = grid.Min(c => c.Y);


			var visualGrid = new string[11, 11];
			for (int i = 0; i < visualGrid.GetLength(0); i++)
			{
				for (int j = 0; j < visualGrid.GetLength(1); j++)
				{
					visualGrid[i, j] = ".";
				}
			}
			
			foreach (var coord in grid)
			{
				var x = GRID_CENTER + coord.X;
				var y = GRID_CENTER + coord.Y;
				visualGrid[x, y] = coord.Color == 1 ? "#" : ".";
			}

			visualGrid[GRID_CENTER + position.X, GRID_CENTER + position.Y] = GetDirectionSymbol(direction);

			Console.Clear();
			for (int i = 0; i < visualGrid.GetLength(0); i++)
			{
				for (int j = 0; j < visualGrid.GetLength(1); j++)
				{
					Console.Write(visualGrid[i, j]);
				}
				Console.WriteLine();
			}
			System.Threading.Thread.Sleep(100);

		}

		private string GetDirectionSymbol(Directions direction) => direction switch
		{
			Directions.Up => ">",
			Directions.Right => "v",
			Directions.Down => "<",
			Directions.Left => "^",
		};
	}
}
