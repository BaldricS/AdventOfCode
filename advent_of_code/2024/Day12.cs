using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace AOC
{
    public record Input202412(string[] Lines);

    [AdventOfCode(2024, 12)]
    public static class Day12_2024
    {
        [MapInput]
        public static Input202412 Map(string[] lines)
        {
            return new(lines);
        }

        [Solver(1)]
        public static long Solve1(Input202412 input)
        {
            HashSet<(int, int)> seen = [];
            return input.Lines.SelectMany((r, rIdx) => r.Select((c, cIdx) => GetRegionCost((rIdx, cIdx), input.Lines, seen))).Select(item => item.Item1 * item.Item2).Sum();
        }

        public static (int, int) GetRegionCost((int, int) startingPoint, string[] grid, HashSet<(int, int)> seen)
        {
            bool isOnGrid((int, int) item) => item.Item1 >= 0 && item.Item1 < grid.Length && item.Item2 >= 0 && item.Item2 < grid[item.Item1].Length;

            if (seen.Contains(startingPoint))
            {
                return (0, 0);
            }

            seen.Add(startingPoint);

            var left = (startingPoint.Item1, startingPoint.Item2 - 1);
            var right = (startingPoint.Item1, startingPoint.Item2 + 1);
            var up = (startingPoint.Item1 - 1, startingPoint.Item2);
            var down = (startingPoint.Item1 + 1, startingPoint.Item2);
            char me = grid[startingPoint.Item1][startingPoint.Item2];

            var perimeter = 4;
            var area = 1;

            if (isOnGrid(left) && grid[left.Item1][left.Item2] == me)
            {
                perimeter -= 1;
                var leftExpand = GetRegionCost(left, grid, seen);

                perimeter += leftExpand.Item1;
                area += leftExpand.Item2;
            }
            if (isOnGrid(right) && grid[right.Item1][right.Item2] == me)
            {
                perimeter -= 1;
                var rightExpand = GetRegionCost(right, grid, seen);

                perimeter += rightExpand.Item1;
                area += rightExpand.Item2;
            }
            if (isOnGrid(up) && grid[up.Item1][up.Item2] == me)
            {
                perimeter -= 1;
                var upExpand = GetRegionCost(up, grid, seen);

                perimeter += upExpand.Item1;
                area += upExpand.Item2;
            }
            if (isOnGrid(down) && grid[down.Item1][down.Item2] == me)
            {
                perimeter -= 1;
                var downExpand = GetRegionCost(down, grid, seen);

                perimeter += downExpand.Item1;
                area += downExpand.Item2;
            }

            return (perimeter, area);
        }

        [Solver(2)]
        public static long Solve2(Input202412 input)
        {
            HashSet<(int, int)> seen = [];
            return input.Lines.SelectMany((r, rIdx) => r.Select((c, cIdx) => GetRegionCost2((rIdx, cIdx), input.Lines, seen))).Select(item =>{
                return item.Item1 * item.Item2;
            } ).Sum();
        }

        public static (int, int) GetRegionCost2((int, int) startingPoint, string[] grid, HashSet<(int, int)> seen)
        {
            bool isOnGrid((int, int) item) => item.Item1 >= 0 && item.Item1 < grid.Length && item.Item2 >= 0 && item.Item2 < grid[item.Item1].Length;

            if (seen.Contains(startingPoint))
            {
                return (0, 0);
            }

            seen.Add(startingPoint);

            var left = (startingPoint.Item1, startingPoint.Item2 - 1);
            var right = (startingPoint.Item1, startingPoint.Item2 + 1);
            var up = (startingPoint.Item1 - 1, startingPoint.Item2);
            var down = (startingPoint.Item1 + 1, startingPoint.Item2);
            char me = grid[startingPoint.Item1][startingPoint.Item2];
            var rightIsMe = isOnGrid(right) && grid[right.Item1][right.Item2] == me;
            var leftIsMe = isOnGrid(left) && grid[left.Item1][left.Item2] == me;
            var upIsMe = isOnGrid(up) && grid[up.Item1][up.Item2] == me;
            var downIsMe = isOnGrid(down) && grid[down.Item1][down.Item2] == me;

            var area = 1;
            var sides = 0;

            if (leftIsMe)
            {
                var leftExpand = GetRegionCost2(left, grid, seen);

                area += leftExpand.Item2;
                sides += leftExpand.Item1;
            }

            if (rightIsMe)
            {
                var rightExpand = GetRegionCost2(right, grid, seen);

                area += rightExpand.Item2;
                sides += rightExpand.Item1;
            }

            if (upIsMe)
            {
                var upExpand = GetRegionCost2(up, grid, seen);

                area += upExpand.Item2;
                sides += upExpand.Item1;
            }

            if (downIsMe)
            {
                var downExpand = GetRegionCost2(down, grid, seen);

                area += downExpand.Item2;
                sides += downExpand.Item1;
            }

            var diagUpRight = (startingPoint.Item1 - 1, startingPoint.Item2 + 1);
            var diagUpLeft = (startingPoint.Item1 - 1, startingPoint.Item2 - 1);
            var diagDownRight = (startingPoint.Item1 + 1, startingPoint.Item2 + 1);
            var diagDownLeft = (startingPoint.Item1 + 1, startingPoint.Item2 - 1);
            var diagUpRightIsMe = isOnGrid(diagUpRight) && grid[diagUpRight.Item1][diagUpRight.Item2] == me;
            var diagUpLeftIsMe = isOnGrid(diagUpLeft) && grid[diagUpLeft.Item1][diagUpLeft.Item2] == me;
            var diagDownRightIsMe = isOnGrid(diagDownRight) && grid[diagDownRight.Item1][diagDownRight.Item2] == me;
            var diagDownLeftIsMe = isOnGrid(diagDownLeft) && grid[diagDownLeft.Item1][diagDownLeft.Item2] == me;

            if (leftIsMe && upIsMe && !diagUpLeftIsMe)
            {
                ++sides;
            }

            if (upIsMe && rightIsMe && !diagUpRightIsMe)
            {
                ++sides;
            }

            if (rightIsMe && downIsMe && !diagDownRightIsMe)
            {
                ++sides;
            }

            if (downIsMe && leftIsMe && !diagDownLeftIsMe)
            {
                ++sides;
            }

            if (!leftIsMe && !upIsMe)
            {
                ++sides;
            }

            if (!leftIsMe && !downIsMe)
            {
                ++sides;
            }

            if (!downIsMe && !rightIsMe)
            {
                ++sides;
            }

            if (!rightIsMe && !upIsMe)
            {
                ++sides;
            }

            return (sides, area);
        }
    }
}