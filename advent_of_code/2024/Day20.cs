using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    public record Input202420()
    {
        public char[][] Maze { get; set; }
    }

    [AdventOfCode(2024, 20)]
    public static class Day20_2024
    {
        [MapInput]
        public static Input202420 Map(string[] lines)
        {
            return new Input202420(){
                Maze = lines.Select(l => l.ToCharArray()).ToArray()
            };
        }

        [Solver(1)]
        public static int Solve1(Input202420 input)
        {
            var start = Find(input.Maze, 'S');
            var end = Find(input.Maze, 'E');
            var noCheating = FindLowestScore(input.Maze, start, end);
            noCheating.Item2.Add(end);

            Dictionary<int, int> savings = [];

            for (int i = 0; i < noCheating.Item2.Count; ++i)
            {
                var next = noCheating.Item2[i];

                for (int j = i + 1; j < noCheating.Item2.Count; ++j)
                {
                    var goal = noCheating.Item2[j];
                    if (Manhatten(next, goal) > 2)
                    {
                        continue;
                    }

                    if (Manhatten(next, goal) == 1)
                    {
                        continue;
                    }

                    int pathCost = i + noCheating.Item2.Count - j + Manhatten(next, goal) - 1;
                    int delta = noCheating.Item1 - pathCost;
                    if (delta >= 100)
                    {
                        savings[delta] = savings.GetValueOrDefault(delta, 0) + 1;
                    }
                }
            }

            return savings.Values.Sum();
        }

        [Solver(2)]
        public static long Solve2(Input202420 input)
        {
            var start = Find(input.Maze, 'S');
            var end = Find(input.Maze, 'E');
            var noCheating = FindLowestScore(input.Maze, start, end);
            noCheating.Item2.Add(end);

            Dictionary<int, int> savings = [];

            for (int i = 0; i < noCheating.Item2.Count; ++i)
            {
                var next = noCheating.Item2[i];

                for (int j = i + 1; j < noCheating.Item2.Count; ++j)
                {
                    var goal = noCheating.Item2[j];
                    if (Manhatten(next, goal) > 20)
                    {
                        continue;
                    }

                    if (Manhatten(next, goal) == 1)
                    {
                        continue;
                    }

                    int pathCost = i + noCheating.Item2.Count - j + Manhatten(next, goal) - 1;
                    int delta = noCheating.Item1 - pathCost;
                    if (delta >= 100)
                    {
                        savings[delta] = savings.GetValueOrDefault(delta, 0) + 1;
                    }
                }
            }

            return savings.Values.Sum();
        }

        public static int Manhatten((int, int) a, (int, int) b)
        {
            return Math.Abs(a.Item2 - b.Item2) + Math.Abs(a.Item1 - b.Item1);
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

        public static (int, List<(int, int)>) FindLowestScore(char[][] maze, (int, int) pos, (int, int) goal)
        {
            PriorityQueue<(int, int), int> openSet = new();
            openSet.Enqueue(pos, 0);

            Dictionary<(int, int), int> gScore = new()
            {
                [pos] = 0
            };
            Dictionary<(int, int), (int, int)> cameFrom = [];

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();               
                if (current.Item1 == goal.Item1 && current.Item2 == goal.Item2)
                {
                    var tmp = current;
                    List<(int, int)> path = [];

                    while (cameFrom.ContainsKey(tmp))
                    {
                        tmp = cameFrom[tmp];
                        path.Add(tmp);
                    }

                    path.Reverse();
                    return (gScore[current], path);
                }

                (int, int)[] moves = [
                    Move(current, '^'),
                    Move(current, '<'),
                    Move(current, 'v'),
                    Move(current, '>')
                ];

                foreach (var next in moves)
                {
                    if (!CanMoveTo(maze, next))
                    {
                        continue;
                    }

                    var moveScore = 1;
                    var tentativeScore = gScore[current] + moveScore;
                    if (tentativeScore < gScore.GetValueOrDefault(next, int.MaxValue))
                    {
                        cameFrom[next] = current;
                        gScore[next] = tentativeScore;
                        openSet.Enqueue(next, tentativeScore);
                    }

                }
            }

            return (int.MaxValue, []);
        }

        public static (int, int) Move((int, int) pos, char dir)
        {
            if (dir == '^')
            {
                return (pos.Item1 - 1, pos.Item2);
            }

            if (dir == 'v')
            {
                return (pos.Item1 + 1, pos.Item2);
            }

            if (dir == '<')
            {
                return (pos.Item1, pos.Item2 - 1);
            }

            return (pos.Item1, pos.Item2 + 1);
        }

        public static bool CanMoveTo(char[][] board, (int, int) pos)
        {
            return pos.Item1 >= 0 && pos.Item1 < board.Length && pos.Item2 >= 0 && pos.Item2 < board[pos.Item1].Length && board[pos.Item1][pos.Item2] != '#';
        }

        public static bool Matches(char[][] board, (int, int) pos, char match)
        {
            return pos.Item1 >= 0 && pos.Item1 < board.Length && pos.Item2 >= 0 && pos.Item2 < board[pos.Item1].Length && board[pos.Item1][pos.Item2] == match;
        }
    }
}