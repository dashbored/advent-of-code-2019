using System;

namespace Day10
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = System.IO.File.ReadAllText(@"C:\dev\slask\adventofcode2019\inputs\day10-1.txt").ToCharArray();
			var map = new Map(input);

		}
	}
}
