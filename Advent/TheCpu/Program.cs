using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Word = System.Numerics.BigInteger;

namespace TheCpu
{
	class Program
	{
		static void Main(string[] args)
		{
			Word max = -1;
			string best = "?";

			foreach (var permutation in GeneratePermutations(9, 8, 7, 6, 5))
			{
				var key = string.Join(",", permutation);
				var result = Compute(permutation);

				Console.WriteLine($"f({key}) = {result}");

				if (result > max)
				{
					max = result;
					best = key;
				}
			}

			Console.WriteLine("Winner:");
			Console.WriteLine($"{best} = {max}");
		}

		static IEnumerable<int[]> GeneratePermutations(params int[] args)
		{
			IEnumerable<int[]> Gen(int[] fix, int[] free)
			{
				foreach (int n in free)
				{
					var fixPlus = fix.Append(n).ToArray();
					var ns = new int[] { n };
					var freeMinus = free.Except(ns).ToArray();

					foreach (var perm in Gen(fixPlus, freeMinus))
					{
						yield return perm;
					}
				}

				if (free.Any() == false)
				{
					yield return fix;
				}
			}

			return Gen(new int[0], args);
		}

		static Word Compute(params int[] args)
		{
			int N = args.Length;
			Word result = -1;

			try
			{
				Bus bus = new Bus(N);
				var pipes = Enumerable.Range(0, N)
					.Select(n => new IOBus(bus, n, (n + 1) % N))
					.ToList();

				List<Task> cluster = new List<Task>();

				foreach (var pipe in pipes)
				{
					int id = cluster.Count;

					pipe.Write(args[id]);
					if (id == 0)
					{
						pipe.Write(0);
					}

					var cpu = new CPU(pipe);

					var task = Task.Run(() =>
					{
						cpu.Load(@"T:\Fredrik\day7-1.txt");
						cpu.Run();

						if (id == 0)
						{
							result = bus.Read(1);							
						}
					});

					cluster.Add(task);
				}

				Task.WaitAll(cluster.ToArray());				
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

			return result;
		}
	}
}
