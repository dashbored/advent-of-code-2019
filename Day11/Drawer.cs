using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day11
{
	public class Drawer
	{
		const int GRID_CENTER = 50;
		public void Draw(List<Coordinate> grid, Coordinate position, Directions direction)
		{
			var visualGrid = new string[GRID_CENTER*2 + 1, GRID_CENTER * 2 + 1];

			for (int i = 0; i < visualGrid.GetLength(0); i++)
			{
				for (int j = 0; j < visualGrid.GetLength(1); j++)
				{
					visualGrid[i, j] = ".";
				}
			}
			
			foreach (var coord in grid)
			{
				var ud = GRID_CENTER + coord.X;
				var lr = GRID_CENTER - coord.Y;
				visualGrid[lr, ud] = coord.Color == 1 ? "#" : "-";
			}

			visualGrid[GRID_CENTER - position.Y, GRID_CENTER + position.X] = GetDirectionSymbol(direction);

			Console.Clear();
			for (int i = 0; i < visualGrid.GetLength(0); i++)
			{
				for (int j = 0; j < visualGrid.GetLength(1); j++)
				{
					Console.Write(visualGrid[i, j]);
				}
				Console.WriteLine();
			}
			System.Threading.Thread.Sleep(10);

		}

		private string GetDirectionSymbol(Directions direction) => direction switch
		{
			Directions.Up => "^",
			Directions.Right => ">",
			Directions.Down => "v",
			Directions.Left => "<",
		};
	}
}
