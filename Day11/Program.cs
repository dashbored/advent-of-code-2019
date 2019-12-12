using System;
using System.Threading.Tasks;
using TheCpu;

namespace Day11
{
	class Program
	{
		static void Main(string[] args)
		{
			var map = new Map();
			var bus = new Bus(2);
			var IOBus = new IOBus(bus, 0, 1);
			var cpu = new CPU(IOBus);
			var IOBus2 = new IOBus(bus, 1, 0);
			//var IOPort = new IORoboPort();
			var robot = new Robot(map, cpu, IOBus2);

			var task = Task.Run(() =>
			{
				robot.Start();
			});

			var task2 = Task.Run(() =>
			{
				cpu.Run();
			});

			Task.WaitAll(task2);
			int paintedCoord = map.GetNumberOfPaintedCoordinates();
			Console.WriteLine(paintedCoord);
		}
	}
}
