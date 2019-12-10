using System;
using System.Collections.Generic;
using System.Linq;

namespace TheCpu
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var cpu = new CPU();
				cpu.Load(@"C:\dev\slask\adventofcode2019\inputs\day9-1.txt");
				//cpu.Load(104, 1125899906842624, 99);
				cpu.Run();

				//Amplify(cpu, 5);

				//CPU[] cpus = Enumerable.Range(0, 5).Select(x => new CPU()).ToArray();
				//AmplifyMaximus(cpus);
				Console.WriteLine($"IC: {cpu.IC}, OUT: {cpu.OUT}");
			}
			catch (DecodeException ex)
			{
				Console.WriteLine($"Decode error: Op Code {ex.OpCode}");
			}
			catch (AddressException ex)
			{
				Console.WriteLine($"Address error: {ex.Address} (0..{ex.RAMSize - 1})");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		private static void Amplify(CPU cpu, int loops)
		{
			List<Stack<int>> phaseSettings = GetPhaseSettings(6, 5);
			int maxVal = 0;
			foreach (var phaseSetting in phaseSettings)
			{
				for (int i = 0; i < loops; i++)
				{
					cpu.Load(3, 52, 1001, 52, -5, 52, 3, 53, 1, 52, 56, 54, 1007, 54, 5, 55, 1005, 55, 26, 1001, 54, -5, 54, 1105, 1, 12, 1, 53, 54, 53, 1008, 54, 0, 55, 1001, 55, 1, 55, 2, 53, 55, 53, 4, 53, 1001, 56, -1, 56, 1005, 56, 6, 99, 0, 0, 0, 0, 10);
					//cpu.IN.Push(cpu.OUT);
					//cpu.IN.Push(phaseSetting.Pop());
					cpu.Run();
				}
				maxVal = Math.Max(maxVal, (int)cpu.OUT);
			}
		}

		private static void AmplifyMaximus(CPU[] cpus)
		{
			foreach (var cpu in cpus)
			{
				cpu.Load(3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26, 27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5);
			}

			List<Stack<int>> phaseSettings = GetPhaseSettings(0, 5);

		}

		private static List<Stack<int>> GetPhaseSettings(int min, int count)
		{
			var output = new List<int[]>();
			Permutate(new List<int>(Enumerable.Range(min, count)), new List<int>(), output);

			var ps = new List<Stack<int>>();
			foreach (var phaseSetting in output)
			{
				ps.Add(new Stack<int>(phaseSetting));
			}
			return ps;
		}

		private static void Permutate(List<int> available, List<int> taken, List<int[]> output)
		{
			if (available.Count < 1)
			{
				output.Add(taken.ToArray());
				return;
			}

			for (int i = 0; i < available.Count; i++)
			{
				var x = available[i];
				taken.Add(x);

				var availableCopy = available.ToList();
				availableCopy.RemoveAt(i);

				var takenCopy = taken.ToList();
				Permutate(availableCopy, takenCopy, output);
				taken.Remove(x);
			}
		}
	}


}
