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
                    break;
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

            for (int s1 = 0, s2 = 1, e = input.Length - 1; s2 < e;)
            {
                int sum = input[s1] + input[s2] + input[e];
                if (sum == 2020)
                {
                    Console.WriteLine($"Day 1 Puzzle 2 Solution: {input[s1] * input[s2] * input[e]}.");
                    break;
                }
                else if (sum > 2020)
                {
                    --e;
                }
                else if ((s2 - s1) == 1)
                {
                    ++s2;
                }
                else
                {
                    int s1Step = input[s1 + 1] - input[s1];
                    int s2Step = input[s2 + 1] - input[s2];

                    if (s1Step < s2Step)
                    {
                        ++s1;
                    }
                    else
                    {
                        ++s2;
                    }
                }
            }
        }
    }
}
