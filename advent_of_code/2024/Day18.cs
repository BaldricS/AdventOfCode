using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC
{
    public record Input202418()
    {
        public int[][] Bytes { get; set; }
    }

    [AdventOfCode(2024, 18)]
    public static class Day18_2024
    {
        [MapInput]
        public static Input202418 Map(string[] lines)
        {
            return new Input202418(){
                Bytes = lines.Select(l => l.Split(",").Select(int.Parse).ToArray()).ToArray()
            };
        }

        [Solver(1)]
        public static int Solve1(Input202418 input)
        {
            var size = 71;
            var bytes = 1024;
            var mem = Enumerable.Range(0, size).Select(_ => Enumerable.Range(0, size).Select(_ => '.').ToArray()).ToArray();

            foreach (var b in input.Bytes.Take(bytes))
            {
                mem[b[0]][b[1]] = '#';
            }
            Print(mem);

            (int, int) start = (0, 0);
            (int, int) goal = (size - 1, size - 1);

            (int cost, var path) = FindLowestScore(mem, start, goal);

            return path.Count;
        }

        public static void Print(char[][] board)
        {
            for (int r = 0; r < board.Length; ++r)
            {
                for (int c = 0; c < board[r].Length; ++c)
                {
                    Console.Write(board[r][c]);
                }
                Console.WriteLine();
            }
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

        [Solver(2)]
        public static (int, int) Solve2(Input202418 input)
        {
            var size = 71;
            var start = 0;
            var end = input.Bytes.Length - 1;

            (int, int) startPos = (0, 0);
            (int, int) goalPos = (size - 1, size - 1);

            while (start <= end)
            {
                int mid = (start + end) / 2;
                var mem = GenMaze(input.Bytes, size, mid);

                (_, var path) = FindLowestScore(mem, startPos, goalPos);
                // Empty path means we need to go down.
                if (path.Count == 0)
                {
                    end = mid - 1;
                }
                else
                {
                    start = mid + 1;
                }
            }

            var b = input.Bytes[start - 1];

            return (b[0], b[1]);
        }

        public static char[][] GenMaze(int[][] bytes, int size, int toTake)
        {
            var mem = Enumerable.Range(0, size).Select(_ => Enumerable.Range(0, size).Select(_ => '.').ToArray()).ToArray();
            foreach (var b in bytes.Take(toTake))
            {
                mem[b[0]][b[1]] = '#';
            }
            return mem;
        }
    }
}