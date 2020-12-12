using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    using ChallengeType = String;

    [AdventOfCode(2020, 11)]
    public static class Day11_Year2020
    {
        public static IEnumerable<(int X, int Y)> Neighbors(int r, int c)
        {
            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    yield return (r + x, c + y);
                }
            }
        }

        public static int CountVisibleNeighbors(int r, int c, List<string> grid)
        {
            int CountSeat(int[] rows, int[] cols)
            {
                for (int r = 0, c = 0; r < rows.Length && c < cols.Length; ++r, ++c)
                {
                    char ch = grid[rows[r]][cols[c]];
                    if (ch == 'L')
                    {
                        return 0;
                    }
                    else if (ch == '#')
                    {
                        return 1;
                    }
                }

                return 0;
            }

            static IEnumerable<int> Range(int start, int end)
            {
                for (int i = start; i < end; ++i)
                {
                    yield return i;
                }
            }

            var row = Enumerable.Repeat(r, grid.Count).ToArray();
            var col = Enumerable.Repeat(c, grid[r].Length).ToArray();
            var botCol = Range(0, c).Reverse().ToArray();
            var topCol = Range(c + 1, grid[r].Length).ToArray();
            var botRow = Range(0, r).Reverse().ToArray();
            var topRow = Range(r + 1, grid.Count).ToArray();

            return CountSeat(row, botCol)
                + CountSeat(row, topCol)
                + CountSeat(botRow, col)
                + CountSeat(topRow, col)
                + CountSeat(botRow, botCol)
                + CountSeat(topRow, botCol)
                + CountSeat(botRow, topCol)
                + CountSeat(topRow, topCol);
        }

        public static int NeighborSeats(int r, int c, List<string> grid) =>
            Neighbors(r, c)
                .Where(pr => pr.X >= 0 && pr.X < grid.Count && pr.Y >= 0 && pr.Y < grid[0].Length)
                .Count(pr => grid[pr.X][pr.Y] == '#');

        public static char ImpatientNeighbor(int r, int c, List<string> grid) =>
            grid[r][c] switch
            {
                'L' => NeighborSeats(r, c, grid) == 0 ? '#' : 'L',
                '#' => NeighborSeats(r, c, grid) >= 4 ? 'L' : '#',
                _ => '.'
            };

        public static char PatientNeighbor(int r, int c, List<string> grid) =>
            grid[r][c] switch
            {
                'L' => CountVisibleNeighbors(r, c, grid) == 0 ? '#' : 'L',
                '#' => CountVisibleNeighbors(r, c, grid) >= 5 ? 'L' : '#',
                _ => '.'
            };

        public static bool AreSameGrid(List<string> one, List<string> two) =>
            one.Zip(two).All(rows => rows.First == rows.Second);

        public static List<string> Settle(List<string> grid, Func<int, int, List<string>, char> move)
        {
            while (true)
            {
                var nextGrid = grid
                    .Select((row, r) => string.Join("", row.Select((_, c) => move(r, c, grid))))
                    .ToList();

                if (AreSameGrid(grid, nextGrid))
                {
                    return nextGrid;
                }

                grid = nextGrid;
            }
        }

        public static int CountOccupied(this List<string> grid) => grid.Sum(row => row.Count(c => c == '#'));

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input) =>
            Settle(input.ToList(), ImpatientNeighbor)
            .CountOccupied();

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input) =>
            Settle(input.ToList(), PatientNeighbor)
            .CountOccupied();
    }
}
