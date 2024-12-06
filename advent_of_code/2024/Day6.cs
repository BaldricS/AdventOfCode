using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace AOC
{
    [AdventOfCode(2024, 6)]
    public static class Day6_2024
    {
        [MapInput]
        public static char[][] Map(string[] lines) => lines.Select(l => l.ToCharArray()).ToArray();

        [Solver(1)]
        public static int Solve1(char[][] input)
        {
            var player = FindPlayer(input);
            var spots = GetAllPlayerVisitedSpots(input, player);
            spots.Add(player);
            return spots.Count;
        }

        [Solver(2)]
        public static int Solve2(char[][] input)
        {
            (int, int) playerLoc = FindPlayer(input);
            var allPlayerLocs = GetAllPlayerVisitedSpots(input, playerLoc);
            int loops = 0;

            foreach (var maybeLoc in allPlayerLocs)
            {
                input[maybeLoc.Item1][maybeLoc.Item2] = '#';
                bool hasLoop = DetectLoop(input, playerLoc, 500);
                input[maybeLoc.Item1][maybeLoc.Item2] = '.';

                if (hasLoop)
                {
                    ++loops;
                }
            }

            return loops;
        }

        public static bool DetectLoop(char[][] input, (int, int) playerLoc, int maxCasts)
        {
            int currentCasts = 0;
            int dirIndex = 0;
            (int, int)[] dirs = [(-1, 0), (0, 1), (1, 0), (0, -1)];

            while (currentCasts++ < maxCasts)
            {
                (int, int) movementDir = dirs[dirIndex++ % 4];
                (int, int)[] directions = CastRay(input, playerLoc, movementDir, '#').ToArray();

                if (input[directions.Last().Item1][directions.Last().Item2] == '.')
                {
                    return false;
                }

                if (directions.Length > 1)
                {
                    playerLoc = directions.SkipLast(1).Last();
                }
            }

            return true;
        }

        public static HashSet<(int, int)> GetAllPlayerVisitedSpots(char[][] input, (int, int) playerLoc)
        {
            int dirIndex = 0;
            (int, int)[] dirs = [(-1, 0), (0, 1), (1, 0), (0, -1)];
            HashSet<(int, int)> seenLocs = [];

            while (true)
            {
                (int, int) movementDir = dirs[dirIndex++ % 4];
                (int, int)[] directions = CastRay(input, playerLoc, movementDir, '#').ToArray();
                foreach (var d in directions.SkipLast(1))
                {
                    seenLocs.Add(d);
                }

                if (input[directions.Last().Item1][directions.Last().Item2] == '.')
                {
                    seenLocs.Add(directions.Last());
                    return seenLocs;
                }

                if (directions.Length > 1)
                {
                    playerLoc = directions.SkipLast(1).Last();
                }
            }
        }

        public static IEnumerable<(int, int)> CastRay(char[][] input, (int, int) current, (int, int) direction, char target)
        {
            (int, int) next = (current.Item1 + direction.Item1, current.Item2 + direction.Item2);
            while (!(next.Item1 < 0 || next.Item1 >= input.Length || next.Item2 < 0 || next.Item2 >= input[next.Item1].Length))
            {
                yield return next;

                if (input[next.Item1][next.Item2] == target)
                {
                    break;
                }

                current = next;
                next = (current.Item1 + direction.Item1, current.Item2 + direction.Item2);
            }
        }

        public static (int, int) FindPlayer(char[][] input)
        {
            for (int r = 0; r < input.Length; ++r)
            {
                for (int c = 0; c < input[r].Length; ++c)
                {
                    if (input[r][c] == '^')
                    {
                        return (r, c);
                    }
                }
            }

            throw new Exception("Doomed");
        }
    }
}