namespace AOC
{
    [AdventOfCode(2020, 3)]
    public static class Day3_2020
    {
        static long CountTreesOnSlope(string[] lines, int slopeColumn, int slopeRow)
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

        [Solver(1)]
        public static long CountSingleSlope(string[] lines) => CountTreesOnSlope(lines, 3, 1);

        [Solver(2)]
        public static long CountAllSlopes(string[] lines) =>
            CountTreesOnSlope(lines, 1, 1) *
            CountTreesOnSlope(lines, 3, 1) *
            CountTreesOnSlope(lines, 5, 1) *
            CountTreesOnSlope(lines, 7, 1) *
            CountTreesOnSlope(lines, 1, 2);
    }
}
