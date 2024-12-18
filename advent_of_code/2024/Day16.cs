using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    public record Input202416(char[][] lines);

    [AdventOfCode(2024, 16)]
    public static class Day16_2024
    {
        [MapInput]
        public static Input202416 Map(string[] lines)
        {
            return new Input202416(lines.Select(l => l.ToCharArray()).ToArray());
        }

        [Solver(1)]
        public static int Solve1(Input202416 input)
        {
            var start = Find(input.lines, 'S');
            var goal = Find(input.lines, 'E');
            var t = FindLowestScore(input.lines, (start.Item1, start.Item2, 'R'), goal);

            return t.Item1;
        }

        public static (int, List<(int, int, char)>) FindLowestScore(char[][] maze, (int, int, char) pos, (int, int) goal)
        {
            PriorityQueue<(int, int, char), int> openSet = new();
            openSet.Enqueue(pos, 0);

            Dictionary<(int, int, char), int> gScore = new()
            {
                [pos] = 0
            };
            Dictionary<(int, int, char), (int, int, char)> cameFrom = [];

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();               
                if (current.Item1 == goal.Item1 && current.Item2 == goal.Item2)
                {
                    var tmp = current;
                    List<(int, int, char)> path = [];

                    while (cameFrom.ContainsKey(tmp))
                    {
                        tmp = cameFrom[tmp];
                        path.Add(tmp);
                    }

                    return (gScore[current], path);
                }

                var newFacings = GetLeftRightAndStraight(current.Item3);
                foreach (var f in newFacings)
                {
                    var next = Move((current.Item1, current.Item2), f);
                    if (!CanMoveTo(maze, next))
                    {
                        continue;
                    }

                    var moveScore = 1;
                    if (f != current.Item3)
                    {
                        moveScore += 1000;
                    }

                    var tentativeScore = gScore[current] + moveScore;
                    var nKey = (next.Item1, next.Item2, f);
                    if (tentativeScore < gScore.GetValueOrDefault(nKey, int.MaxValue))
                    {
                        cameFrom[nKey] = current;
                        gScore[nKey] = tentativeScore;
                        openSet.Enqueue(nKey, tentativeScore);
                    }

                }
            }

            return (int.MaxValue, []);
        }

        public static (int, int) Move((int, int) pos, char facing)
        {
            if (facing == 'U')
            {
                return (pos.Item1 - 1, pos.Item2);
            }

            if (facing == 'D')
            {
                return (pos.Item1 + 1, pos.Item2);
            }

            if (facing == 'L')
            {
                return (pos.Item1, pos.Item2 - 1);
            }

            return (pos.Item1, pos.Item2 + 1);
        }

        public static char[] GetLeftRightAndStraight(char facing)
        {
            if (facing == 'U')
            {
                return ['U', 'L', 'R'];
            }

            if (facing == 'D')
            {
                return ['D', 'L', 'R'];
            }

            if (facing == 'L')
            {
                return ['L', 'U', 'D'];
            }

            return ['R', 'U', 'D'];
        }

        public static bool CanMoveTo(char[][] board, (int, int) pos)
        {
            return pos.Item1 >= 0 && pos.Item1 < board.Length && pos.Item2 >= 0 && pos.Item2 < board[pos.Item1].Length && board[pos.Item1][pos.Item2] != '#';
        }

        public static (int, int) Find(char[][] board, char target)
        {
            for (int r = 0; r < board.Length; ++r)
            {
                for (int c = 0; c < board[r].Length; ++c)
                {
                    if (board[r][c] == target)
                    {
                        return (r, c);
                    }
                }
            }

            return (-1, -1);
        }

        [Solver(2)]
        public static long Solve2(Input202416 input)
        {
            var start = Find(input.lines, 'S');
            var goal = Find(input.lines, 'E');
            var (cost, path) = FindLowestScore(input.lines, (start.Item1, start.Item2, 'R'), goal);
            HashSet<(int, int)> seats = [(start.Item1, start.Item2), (goal.Item1, goal.Item2)];
            List<(int, int)> toTry = path.Select(i => (i.Item1, i.Item2)).ToList();

            while (toTry.Count > 0)
            {
                var seat = toTry[^1];
                toTry.RemoveAt(toTry.Count - 1);

                    input.lines[seat.Item1][seat.Item2] = '#';
                    var (cost2, path2) = FindLowestScore(input.lines, (start.Item1, start.Item2, 'R'), goal);
                    input.lines[seat.Item1][seat.Item2] = '.';

                    if (cost2 == cost)
                    {
                        toTry.AddRange(path2.Select(p => (p.Item1, p.Item2)).Where(s => !seats.Contains(s)));
                    }
            }

            return seats.Count;
        }
    }
}