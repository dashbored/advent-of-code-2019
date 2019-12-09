using System;

namespace TheCpu
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var cpu = new CPU();
				
				//cpu.Load(@"T:\Fredrik\input5.txt");				
				//cpu.Load(3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0);
				Amplify(cpu, 5);
				//cpu.Run();
				Console.WriteLine($"IC: {cpu.IC}");
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
			for (int i = 0; i < loops; i++)
			{
				cpu.Load(3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33, 1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0);
				cpu.Run();
			}
		}
	}
}
