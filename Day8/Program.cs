using System;
using System.Collections.Generic;
using System.Linq;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            var encodedInput = System.IO.File.ReadAllText(@"E:\Downloads\dev\advent-of-code-2019\inputs\day8-1.txt");
            var layeredEncodedInput = encodedInput.Split(encodedInput.Length / (25*6)).ToArray();
            List<int> count = new List<int>();
            foreach (var input in layeredEncodedInput)
            {
                count.Add(CountOccurrences(input, 0));
            }
            var layerIndexWithLeastZeros = count.IndexOfMin();

            var layerOfInterest = layeredEncodedInput[layerIndexWithLeastZeros];
            var output = CountOccurrences(layerOfInterest, 1) * CountOccurrences(layerOfInterest, 2);
            var output2 = CountOccurrences(layeredEncodedInput[13], 1) * CountOccurrences(layeredEncodedInput[13], 2);

        }

        static int CountOccurrences(string input, int number)
        {
            var character = (char)(number + 48);
            return input.Count(i => i == character);
        }
    }

    public static class Extensions
    {
        public static IEnumerable<string> Split(this string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        public static int IndexOfMin(this IList<int> self)
        {
            int min = self[0];
            int minIndex = 0;

            for (int i = 1; i < self.Count; ++i)
            {
                if (self[i] < min)
                {
                    min = self[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }
    }
}
