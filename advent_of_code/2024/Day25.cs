using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace AOC
{
    public record Input252525()
    {
        public List<int[]> Keys { get; set; }
        public List<int[]> Locks { get; set; }
    }

    [AdventOfCode(2024, 25)]
    public static class Day25_2025
    {
        [MapInput]
        public static Input252525 Map(string[] lines)
        {
            List<int[]> keys = [];
            List<int[]> locks = [];

            for (int i = 0; i < lines.Length; i += 8)
            {
                int[] heights = [-1, -1, -1, -1, -1];
                for (int c = 0; c < 5; ++c)
                {
                    for (int h = 0; h < 7; ++h)
                    {
                        if (lines[i + h][c] == '#')
                        {
                            ++heights[c];
                        }
                    }
                }

                if (lines[i] == "#####")
                {
                    locks.Add(heights);
                }
                else
                {
                    keys.Add(heights);
                }
            }

            return new Input252525(){
                Keys = keys,
                Locks = locks
            };
        }

        [Solver(1)]
        public static long Solve1(Input252525 input)
        {
            int fits = 0;

            foreach (var k in input.Keys)
            {
                foreach (var l in input.Locks)
                {
                    bool doesFit = true;
                    for (int c = 0; c < 5; ++c)
                    {
                        if (k[c] + l[c] >= 6)
                        {
                            doesFit = false;
                            break;
                        }
                    }

                    if (doesFit)
                    {
                        ++fits;
                    }
                }
            }

            return fits;
        }

        [Solver(2)]
        public static long Solve2(Input252525 input)
        {
            return 1;
        }
    }
}