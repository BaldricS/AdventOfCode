using System;

using AOC;

namespace Day3
{
    class Program
    {
        static int CountTreesOnSlope(string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Length;
            int slopeOver = 3;
            int treesHit = 0;

            int currentCol = 0;
            for (int r = 1; r < rows; ++r)
            {
                currentCol += slopeOver;

                if (lines[r][currentCol % cols] == '#')
                {
                    ++treesHit;
                }
            }

            return treesHit;
        }

        static void Main(string[] args)
        {
            var puzzle1 = new AdventOfCode(3, 1);
            puzzle1.Run(CountTreesOnSlope);
        }
    }
}
