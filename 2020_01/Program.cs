using System;

using AOC;

namespace AdventOfCode_2020
{
    class Program
    {
        static int SolvePuzzle1(int[] input)
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

        static int SolvePuzzle2(int[] input)
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

        static void Main()
        {
            var puzzle1 = new AdventOfCode(1, 1);
            puzzle1.Run(int.Parse, SolvePuzzle1);

            var puzzle2 = new AdventOfCode(1, 2);
            puzzle2.Run(int.Parse, SolvePuzzle2);
        }
    }
}
