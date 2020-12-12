using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

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
            int occupied = 0;
            for (int i = c - 1; i >= 0; --i)
            {
                if (grid[r][i] == 'L')
                {
                    break;
                }
                if (grid[r][i] == '#')
                {
                    ++occupied;
                    break;
                }
            }

            for (int i = c + 1; i < grid[r].Length; ++i)
            {
                if (grid[r][i] == 'L')
                {
                    break;
                }
                if (grid[r][i] == '#')
                {
                    ++occupied;
                    break;
                }
            }

            for (int i = r - 1; i >= 0; --i)
            {
                if (grid[i][c] == 'L')
                {
                    break;
                }
                if (grid[i][c] == '#')
                {
                    ++occupied;
                    break;
                }
            }

            for (int i = r + 1; i < grid.Count; ++i)
            {
                if (grid[i][c] == 'L')
                {
                    break;
                }
                if (grid[i][c] == '#')
                {
                    ++occupied;
                    break;
                }
            }

            for (int i = r - 1, j = c - 1; i >= 0 && j >= 0; --i, --j)
            {
                if (grid[i][j] == 'L')
                {
                    break;
                }
                if (grid[i][j] == '#')
                {
                    ++occupied;
                    break;
                }
            }

            for (int i = r + 1, j = c + 1; i < grid.Count && j < grid[r].Length; ++i, ++j)
            {
                if (grid[i][j] == 'L')
                {
                    break;
                }
                if (grid[i][j] == '#')
                {
                    ++occupied;
                    break;
                }
            }

            for (int i = r - 1, j = c + 1; i >= 0 && j < grid[r].Length; --i, ++j)
            {
                if (grid[i][j] == 'L')
                {
                    break;
                }
                if (grid[i][j] == '#')
                {
                    ++occupied;
                    break;
                }
            }

            for (int i = r + 1, j = c - 1; i < grid.Count && j >= 0 ; ++i, --j)
            {
                if (grid[i][j] == 'L')
                {
                    break;
                }
                if (grid[i][j] == '#')
                {
                    ++occupied;
                    break;
                }
            }

            return occupied;
        }

        public static int NeighborSeats(int r, int c, List<string> grid) =>
            Neighbors(r, c)
                .Where(pr => pr.X >= 0 && pr.X < grid.Count && pr.Y >= 0 && pr.Y < grid[0].Length)
                .Count(pr => grid[pr.X][pr.Y] == '#');


        public static char NextMove(int r, int c, List<string> grid) =>
            grid[r][c] switch
            {
                'L' => NeighborSeats(r, c, grid) == 0 ? '#' : 'L',
                '#' => NeighborSeats(r, c, grid) >= 4 ? 'L' : '#',
                _ => '.'
            };

        public static char NextMove2(int r, int c, List<string> grid) =>
            grid[r][c] switch
            {
                'L' => CountVisibleNeighbors(r, c, grid) == 0 ? '#' : 'L',
                '#' => CountVisibleNeighbors(r, c, grid) >= 5 ? 'L' : '#',
                _ => '.'
            };

        public static bool AreSameGrid(List<string> one, List<string> two) =>
            one.Zip(two).All(rows => rows.First == rows.Second);

        public static void PrintGrid(List<string> grid)
        {
            foreach (var row in grid)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine();
        }

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input)
        {
            var grid = input.ToList();
            while (true)
            {
                var nextGrid = grid
                    .Select((row, r) => string.Join("", row.Select((_, c) => NextMove(r, c, grid))))
                    .ToList();

                if (AreSameGrid(grid, nextGrid))
                {
                    return nextGrid.Sum(row => row.Count(c => c == '#'));
                }

                grid = nextGrid;
            }
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input)
        {
            var grid = input.ToList();
            while (true)
            {
                var nextGrid = grid
                    .Select((row, r) => string.Join("", row.Select((_, c) => NextMove2(r, c, grid))))
                    .ToList();

                if (AreSameGrid(grid, nextGrid))
                {
                    return nextGrid.Sum(row => row.Count(c => c == '#'));
                }

                grid = nextGrid;
            }
        }
    }
}
