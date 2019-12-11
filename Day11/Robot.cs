using System;
using System.Collections.Generic;
using System.Text;
using TheCpu;
using Word = System.Numerics.BigInteger;


namespace Day11
{
	public enum Directions
	{
		Up,
		Right,
		Down,
		Left
	}

	class Robot
	{
		private Word instr { get; set; }
		private bool isMoveTurn = false;
		private Directions direction = Directions.Up;
		private Drawer drawer = new Drawer();

		private CPU Cpu { get; set; }
		private IOPort Port { get; set; }
		private Map Map { get; set; }
		private Coordinate Position { get; set; }
		private List<Coordinate> Grid() => Map.GetGrid();

		public Robot(Map map, CPU cpu, IOPort port)
		{
			Port = port;
			Map = map;
			Position = Map.GetStartingPosition();
			Cpu = cpu;
			Cpu.Load(@"\\fs\Temp\Fredrik\day11-1.txt");
		}

		internal void Start()
		{
			drawer.Draw(Grid(), Position, direction);
			Port.Write(Position.Color);
			Read();
		}

		private void Read()
		{
			while (true)
			{
				instr = Port.Read();
				if (isMoveTurn)
				{
					Move((int)instr);
					isMoveTurn = false;
				}
				else
				{
					Paint((int)instr);
					drawer.Draw(Grid(), Position, direction);
					isMoveTurn = true;
				}
			}
		}

		private void Paint(int color)
		{
			Position.SetColor(color);
			Port.Write(Position.Color);
		}

		private void Move(int instruction)
		{
			ChangeDirection(instruction);
			var newPostition = GetNewPosition();
			SetPosition(newPostition);
		}

		private Coordinate GetNewPosition() => direction switch
		{
			Directions.Up => Map.GetCoordinate(Position.X, Position.Y + 1),
			Directions.Right => Map.GetCoordinate(Position.X + 1, Position.Y),
			Directions.Down => Map.GetCoordinate(Position.X, Position.Y - 1),
			Directions.Left => Map.GetCoordinate(Position.X - 1, Position.Y),
			_ => throw new Exception("Invalid direction"),
		};


		private void ChangeDirection(int instruction)
		{
			var newDirection = instruction == 0 ? (int)direction - 1 : (int)direction + 1;
			if (newDirection < 0)
			{
				newDirection = 3;
			}
			else if (newDirection == 4)
			{
				newDirection = 0;
			}

			direction = (Directions)newDirection;
		}

		private void SetPosition(Coordinate coordinate)
		{
			Position = coordinate;
		}

	}
}
