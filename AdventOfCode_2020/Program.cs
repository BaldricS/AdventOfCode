using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode_2020
{
    class Program
    {
        static int[] GetInput(string filename)
        {
            var path = Path.GetFullPath(filename);
            var lines = File.ReadAllLines(path);

            return lines.Select(int.Parse).ToArray();
        }

        static void Main()
        {
            var input = GetInput("inputs/day1-1.txt");
            Array.Sort(input);

            for (int s = 0, e = input.Length - 1; s < e;)
            {
                int sum = input[s] + input[e];
                if (sum == 2020)
                {
                    Console.WriteLine($"Day 1 Puzzle 1 Solution: {input[s] * input[e]}.");
                    return;
                }
                else if (sum > 2020)
                {
                    --e;
                }
                else
                {
                    ++s;
                }
            }
        }
    }
}
