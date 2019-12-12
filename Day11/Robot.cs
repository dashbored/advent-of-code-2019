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
        private bool enableDrawing { get; set; }

        private CPU Cpu { get; set; }
        private IOPort Port { get; set; }
        private Map Map { get; set; }
        private Coordinate Position { get; set; }
        private List<Coordinate> Grid() => Map.GetGrid();

        public Robot(Map map, CPU cpu, IOPort port, bool enableDrawing = false)
        {
            Port = port;
            Map = map;
            Position = Map.GetStartingPosition();
            Cpu = cpu;
            Cpu.Load(@"E:\Downloads\dev\advent-of-code-2019\inputs\day11-1.txt");

            this.enableDrawing = enableDrawing;
        }

        internal void Start()
        {
            UpdateGUI();
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
                    UpdateGUI();
                    ReadColor();
                }
                else
                {
                    Paint((int)instr);
                    isMoveTurn = true;
                }
            }
        }

        private void ReadColor()
        {
            Port.Write(Position.Color);
        }

        private void UpdateGUI()
        {
            if (enableDrawing)
            {
                drawer.Draw(Grid(), Position, direction);
            }
        }

        private void Paint(int color)
        {
            Position.SetColor(color);
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
