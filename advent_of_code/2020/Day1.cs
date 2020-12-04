using System;

namespace AOC
{
    [AdventOfCode(2020, 1)]
    public static class Day1_2020
    {
        [MapInput]
        public static int MapLine(string line) => int.Parse(line);

        [Solver(1)]
        public static int Solve1(int[] input)
        {
            Array.Sort(input);

            for (int s = 0, e = input.Length - 1; s < e;)
            {
                int sum = input[s] + input[e];
                if (sum == 2020)
                {
                    return input[s] * input[e];
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

            return -1;
        }

        [Solver(2)]
        public static int Solve2(int[] input)
        {
            Array.Sort(input);

            for (int s1 = 0, s2 = 1, e = input.Length - 1; s2 < e;)
            {
                int sum = input[s1] + input[s2] + input[e];
                if (sum == 2020)
                {
                    return input[s1] * input[s2] * input[e];
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

            return -1;
        }
    }
}