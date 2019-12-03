using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
	class Program
	{
		static void Main(string[] args)
		{
			var wires = new List<Wire>();
			using (StreamReader sr = new StreamReader(@"C:\dev\slask\adventofcode2019\inputs\day3-1.txt"))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					var wire = new Wire();
					var instructions = line.Split(',');
					foreach (var instruction in instructions)
					{
						wire.AddCoordinates(ParseInstructions(instruction, wire.LatestCoordinate()));
					}
					wires.Add(wire);
				}
			}

			var shortestDistance = FindShortestDistanceManhattanPath(wires[0], wires[1]);
			var shortestLength = FindShortestLengthManhattanPath(wires[0], wires[1]);
			Console.WriteLine(shortestDistance);
		}

		private static int FindShortestLengthManhattanPath(Wire wire1, Wire wire2)
		{
			var crossedCoordinates = wire1.Coordinates.Intersect(wire2.Coordinates).ToList();
			var length = new List<int>();
			foreach (var coordinate in crossedCoordinates)
			{
				int lengthToCoordinate1 = wire1.GetLengthToCoordinate(coordinate);
				if (lengthToCoordinate1 == 0)
				{
					continue;
				}
				int lengthToCoordinate2 = wire2.GetLengthToCoordinate(coordinate);

				length.Add(lengthToCoordinate1 + lengthToCoordinate2);
			}
			return length.Min();
		}

		private static int FindShortestDistanceManhattanPath(Wire wire1, Wire wire2)
		{
			//var crossedPaths = wire1.Coordinates.Any(e => e.Equals())
			var crossedCoordinates = wire1.Coordinates.Intersect(wire2.Coordinates).ToList();
			var distances = new List<int>();
			foreach (var coordinate in crossedCoordinates)
			{
				distances.Add(coordinate.ManhattanDistanceFrom(0, 0));
			}
			return distances.Min();

		}

		private static List<Coordinate> ParseInstructions(string instruction, Coordinate startingCoordinate)
		{
			var direction = instruction[0];
			var length = int.Parse(instruction.Substring(1));
			var coordinates = new List<Coordinate>();
			var prevCoord = startingCoordinate;

			for (int i = 0; i < length; i++)
			{
				var coordinate = direction switch
				{
					'R' => new Coordinate { X = prevCoord.X + 1, Y = prevCoord.Y },
					'D' => new Coordinate { X = prevCoord.X, Y = prevCoord.Y - 1},
					'L' => new Coordinate { X = prevCoord.X - 1, Y = prevCoord.Y },
					'U' => new Coordinate { X = prevCoord.X, Y = prevCoord.Y + 1},
					_ => throw new Exception()
				};
				coordinates.Add(coordinate);
				prevCoord = coordinate;
			}
			return coordinates;
		}
	}

	public struct Coordinate
	{
		public int X { get; set; }
		public int Y { get; set; }

		public override bool Equals(object obj)
		{
			
			if (!(obj is Coordinate))
			{
				return false;
			}
			var coord = (Coordinate)obj;
			return (coord.X == X && coord.Y == Y);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y);
		}

		internal int ManhattanDistanceFrom(int x, int y)
		{
			return (Math.Abs(X - x) + Math.Abs(Y - y)); 
		}
	}

	public class Wire
	{
		public List<Coordinate> Coordinates { get; set; }
		public Wire()
		{
			Coordinates = new List<Coordinate>();
			Coordinates.Add(new Coordinate { X = 0, Y = 0 });
		}

		internal void AddCoordinates(List<Coordinate> coordinates)
		{
			foreach (var coordinate in coordinates)
			{
				Coordinates.Add(coordinate);
			}
		}

		internal Coordinate LatestCoordinate()
		{
			return Coordinates.LastOrDefault();
		}

		internal int GetLengthToCoordinate(Coordinate coordinate)
		{
			var index = Coordinates.IndexOf(coordinate);
			if (index == 0)
			{
				return 0;
			}

			int x = 0;
			int y = 0;

			for (int i = 0; i < index; i++)
			{
				x += Math.Abs(Coordinates[i + 1].X - Coordinates[i].X);
				y += Math.Abs(Coordinates[i + 1].Y - Coordinates[i].Y);
			}
			return x + y;
		}
	}
}
