using System;
using System.IO;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader(@"E:\Downloads\dev\adventofcode2019\inputs\day1-1.txt"))
            {
                //RegularFuelCalculator(sr);
                int fuel = RecursiveFuelCounter(sr);
                Console.WriteLine($"Total fuel needed: {fuel}");
            }
        }

        static int RegularFuelCalculator(StreamReader sr)
        {
            string line;
            int fuelCounter = 0;
            while ((line = sr.ReadLine()) != null)
            {
                int mass = int.Parse(line);
                var reqFuel = (mass / 3) - 2;
                fuelCounter += reqFuel;
                Console.WriteLine($"Module mass: {mass}, Fuel req.: {reqFuel}, Total fuel req.: {fuelCounter}");
            }
            return fuelCounter;
        }

        static int RecursiveFuelCounter(StreamReader sr)
        {
            string line;
            int fuelCounter = 0;
            while ((line = sr.ReadLine()) != null)
            {
                int mass = int.Parse(line);
                var reqFuel = FuelCalculator(mass);
                Console.WriteLine($"Recursion done, total fuel: {reqFuel}");
                fuelCounter += reqFuel;
            }
            return fuelCounter;

            static int FuelCalculator(int mass)
            {
                var reqFuel = (mass / 3) - 2;
                Console.WriteLine($"Req fuel: {reqFuel}");
                if (reqFuel > 0)
                {
                    reqFuel += FuelCalculator(reqFuel);
                    return reqFuel;
                }
                return 0;
            }
        }
    }
}
