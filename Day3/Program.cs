using System;

using AOC;

namespace Day3
{
    class Program
    {
        static long CountTreesOnSlope(string[] lines, int slopeRow, int slopeColumn)
        {
            int rows = lines.Length;
            int cols = lines[0].Length;
            long treesHit = 0;

            int currentCol = 0;
            for (int r = slopeRow; r < rows; r += slopeRow)
            {
                currentCol += slopeColumn;

                if (lines[r][currentCol % cols] == '#')
                {
                    ++treesHit;
                }
            }

            return treesHit;
        }

        static long CountAllSlopes(string[] lines) =>
            CountTreesOnSlope(lines, 1, 1) *
            CountTreesOnSlope(lines, 1, 3) *
            CountTreesOnSlope(lines, 1, 5) *
            CountTreesOnSlope(lines, 1, 7) *
            CountTreesOnSlope(lines, 2, 1);

        static void Main(string[] args)
        {
            var puzzle1 = new AdventOfCode(3, 1);
            puzzle1.Run(lines => CountTreesOnSlope(lines, 1, 3));

            var puzzle2 = new AdventOfCode(3, 2);
            puzzle2.Run(CountAllSlopes);
        }
    }
}
