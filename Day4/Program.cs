using System;
using System.Collections.Generic;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            int from = 367479;
            int to = 893698;

            //int current = from;

            List<int> possiblePasswords = new List<int>();

            for (int current = from; current <= to; current++)
            {
                bool result = CheckDouble(current);
                if (!result)
                {
                    continue;
                }
                result = CheckIncreasing(current);
                if (result)
                {
                    possiblePasswords.Add(current);
                }
            }

        }

        private static bool CheckIncreasing(int current)
        {
            var currentString = current.ToString();
            for (int i = 0; i < currentString.Length - 1; i++)
            {
                int leftDigit = int.Parse(currentString[i].ToString());
                int rightDigit = int.Parse(currentString[i + 1].ToString());
                if (leftDigit > rightDigit)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CheckDouble(int current)
        {
            var currentString = current.ToString();
            for (int i = 0; i < currentString.Length - 1; i++)
            {
                if (currentString[i] == currentString[i + 1])
                {
                    if (i < currentString.Length - 2)
                    {
                        if (currentString[i + 1] == currentString[i + 2])
                        {
                            continue;
                        }
                    }
                    if (i > 0)
                    {
                        if (currentString[i - 1] == currentString[i])
                        {
                            continue;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
