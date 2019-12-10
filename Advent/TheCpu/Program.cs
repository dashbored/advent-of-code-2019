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
            //List<Stack<int>> phaseSettings = GetPhaseSettings();
            Stack<int> phaseSetting = new Stack<int>(new int[5] { 6, 5, 8, 7, 9 });

            int maxVal = 0;
            //foreach (var phaseSetting in phaseSettings)
            //{
            for (int i = 0; i < loops; i++)
            {
                cpu.Load(3, 52, 1001, 52, -5, 52, 3, 53, 1, 52, 56, 54, 1007, 54, 5, 55, 1005, 55, 26, 1001, 54, -5, 54, 1105, 1, 12, 1, 53, 54, 53, 1008, 54, 0, 55, 1001, 55, 1, 55, 2, 53, 55, 53, 4, 53, 1001, 56, -1, 56, 1005, 56, 6, 99, 0, 0, 0, 0, 10);
                cpu.IN.Push(cpu.OUT);
                cpu.IN.Push(phaseSetting.Pop());
                cpu.Run();
            }
            maxVal = Math.Max(maxVal, (int)cpu.OUT);
            //}
        }

        private static void Amplify(CPU[] cpus)
        {
            var ampA = cpus[0];
            var ampB = cpus[1];
            var ampC = cpus[2];
            var ampD = cpus[3];
            var ampE = cpus[4];

        }

        private static List<Stack<int>> GetPhaseSettings()
        {
            int[] values = new int[] { 5, 6, 7, 8, 9 };
            List<int[]> phaseSettings = new List<int[]>();
            Permutations.ForAllPermutation(values, (vals) =>
            {
                int[] order = new int[5] { vals[0], vals[1], vals[2], vals[3], vals[4] };
                phaseSettings.Add(order);
                return false;
            });
            List<Stack<int>> ps = new List<Stack<int>>();
            foreach (var phaseSetting in phaseSettings)
            {
                ps.Add(new Stack<int>(phaseSetting));
            }
            return ps;
        }
    }


}
