using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2020, 1)]
    public static class Day1_2020
    {
        [MapInput]
        public static IEnumerable<int> Map(string[] lines) => lines.Select(int.Parse);

        [Solver(1)]
        public static int Solve1(IEnumerable<int> nums)
        {
            var input = nums.ToSortedList();

            for (int s = 0, e = input.Count - 1; s < e;)
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
        public static int Solve2(IEnumerable<int> nums)
        {
            var input = nums.ToSortedList();

            for (int s1 = 0, s2 = 1, e = input.Count - 1; s2 < e;)
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