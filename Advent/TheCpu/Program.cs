using System;
using System.Collections.Generic;
using WpfPermutations;

namespace TheCpu
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var cpu = new CPU();
                Amplify(cpu, 5);
                //cpu.Run();
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
            Stack<int>[] phaseSettings = null;
            var s = GetPhaseSettings();
            //var phaseSettings = new Stack<int>(new int[] { 0, 4, 1, 3, 2 });
            foreach (var phaseSetting in phaseSettings)
            {
                for (int i = 0; i < loops; i++)
                {
                    //cpu.Load(3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33, 1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0);
                    cpu.Load(@"E:\Downloads\dev\advent-of-code-2019\inputs\day7-1.txt");
                    cpu.IN.Push((int)cpu.OUT);
                    cpu.IN.Push(phaseSetting.Pop());
                    cpu.Run();
                }
            }
        }

        private static List<int[]> GetPhaseSettings()
        {
            int[] values = new int[] { 0, 1, 2, 3, 4 };
            List<int[]> phaseSettings = new List<int[]>();
            Permutations.ForAllPermutation(values, (vals) =>
            {
                phaseSettings.Add(vals);
                return false;
            });
            return phaseSettings;
        }
    }


}
