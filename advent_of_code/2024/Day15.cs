using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Transactions;

namespace AOC
{
    public record Input202415(List<List<char>> Board, string Movement);

    [AdventOfCode(2024, 15)]
    public static class Day15_2024
    {
        [MapInput]
        public static Input202415 Map(string[] lines)
        {
            List<List<char>> board = [];
            int i = 0;
            for (; i < lines.Length; ++i)
            {
                if (lines[i] == "")
                {
                    break;
                }

                board.Add([.. lines[i]]);
            }

            return new Input202415(board, string.Join("", lines.Skip(i + 1)));
        }

        [Solver(1)]
        public static int Solve1(Input202415 input)
        {
            (int, int) robot = FindRobot(input.Board);

            foreach (var dir in input.Movement)
            {
                robot = PerformMove(input.Board, robot, dir);
            }

            PrintBoard(input.Board);
            int sum = 0;
            for (int r = 0; r < input.Board.Count; ++r)
            {
                for (int c = 0; c < input.Board[r].Count; ++c)
                {
                    if (input.Board[r][c] == 'O')
                    {
                        sum += r * 100 + c;
                    }
                }
            }

            return sum;
        }

        public static void PrintBoard(List<List<char>> board)
        {
            var sb = new StringBuilder();
            for (int r = 0; r < board.Count; ++r)
            {
                for (int c = 0; c < board[r].Count; ++c)
                {
                    sb.Append(board[r][c]);
                }

                sb.AppendLine();
            }

            Console.Write(sb.ToString());
        }

        public static (int, int) PerformMove(List<List<char>> board, (int, int) current, char dir)
        {
            var next = NextPosition(dir, current);
            if (!IsOnBoard(board, next))
            {
                return current;
            }

            if (board[next.Item1][next.Item2] == '#')
            {
                return current;
            }

            if (board[next.Item1][next.Item2] == '.')
            {
                Swap(board, current, next);
                return next;
            }

            // Try to move box
            PerformMove(board, next, dir);

            // Try to move me
            if (board[next.Item1][next.Item2] == '.')
            {
                return PerformMove(board, current, dir);
            }

            return current;
        }

        public static void Swap(List<List<char>> board, (int, int) current, (int, int) next)
        {
            char tmp = board[current.Item1][current.Item2];
            board[current.Item1][current.Item2] = board[next.Item1][next.Item2];
            board[next.Item1][next.Item2] = tmp;
        }

        public static (int, int) FindRobot(List<List<char>> board)
        {
            for (int r = 0; r < board.Count; ++r)
            {
                for (int c = 0; c < board[r].Count; ++c)
                {
                    if (board[r][c] == '@')
                    {
                        return (r, c);
                    }
                }
            }

            return (-1, -1);
        }

        public static (int, int) NextPosition(char dir, (int, int) p)
        {
            if (dir == '^')
            {
                return (p.Item1 - 1, p.Item2);
            }

            if (dir == '<')
            {
                return (p.Item1, p.Item2 - 1);
            }

            if (dir == 'v')
            {
                return (p.Item1 + 1, p.Item2);
            }

            return (p.Item1, p.Item2 + 1);
        }

        public static bool IsOnBoard(List<List<char>> board, (int, int) p)
        {
            return p.Item1 >= 0 && p.Item1 < board.Count && p.Item2 >= 0 && p.Item2 < board[p.Item1].Count;
        }

        [Solver(2)]
        public static long Solve2(Input202415 input)
        {
            List<List<char>> newBoard = [];

            for (int r = 0; r < input.Board.Count; ++r)
            {
                List<char> row = [];

                for (int c = 0; c < input.Board[r].Count; ++c)
                {
                    if (input.Board[r][c] == '#')
                    {
                        row.Add('#');
                        row.Add('#');
                    }
                    if (input.Board[r][c] == 'O')
                    {
                        row.Add('[');
                        row.Add(']');
                    }
                    if (input.Board[r][c] == '.')
                    {
                        row.Add('.');
                        row.Add('.');
                    }
                    if (input.Board[r][c] == '@')
                    {
                        row.Add('@');
                        row.Add('.');
                    }
                }

                newBoard.Add(row);
            }

            (int, int) robot = FindRobot(newBoard);

            foreach (var dir in input.Movement)
            {
                List<List<char>> snapshot = newBoard.Select(c => c.ToList()).ToList();
                var oldRobot = robot;
                robot = PerformMove2(newBoard, robot, dir);
                if (oldRobot == robot)
                {
                    newBoard = snapshot;
                }
            }

            PrintBoard(newBoard);

            long sum = 0;
            for (int r = 0; r < newBoard.Count; ++r)
            {
                for (int c = 0; c < newBoard[r].Count; ++c)
                {
                    if (newBoard[r][c] == '[')
                    {
                        sum += r * 100L + c;
                    }
                }
            }

            return sum;
        }

        public static (int, int) PerformMove2(List<List<char>> board, (int, int) current, char dir)
        {
            var next = NextPosition(dir, current);
            if (!IsOnBoard(board, next))
            {
                return current;
            }

            if (board[next.Item1][next.Item2] == '#')
            {
                return current;
            }

            if (dir == '<' || dir == '>')
            {
                return PerformMove(board, current, dir);
            }

            // Move box seeing right side
            if (board[next.Item1][next.Item2] == ']')
            {
                var leftSide = (next.Item1, next.Item2 - 1);
                var rightSideNext = NextPosition(dir, next);
                var leftSideNext = NextPosition(dir, leftSide);

                if (IsWall(board, rightSideNext) || IsWall(board, leftSideNext))
                {
                    return current;
                }

                if (IsBox(board, leftSideNext))
                {
                    PerformMove2(board, leftSide, dir);
                }

                if (IsBox(board, rightSideNext))
                {
                    PerformMove2(board, next, dir);
                }

                if (IsEmptySpot(board, rightSideNext) && IsEmptySpot(board, leftSideNext))
                {
                    Swap(board, leftSide, leftSideNext);
                    Swap(board, next, rightSideNext);

                    return PerformMove2(board, current, dir);
                }

                return current;
            }

            // Move front of box
            if (board[next.Item1][next.Item2] == '[')
            {
                var rightSide = (next.Item1, next.Item2 + 1);
                var leftSideNext = NextPosition(dir, next);
                var rightSideNext = NextPosition(dir, rightSide);

                if (IsWall(board, rightSideNext) || IsWall(board, leftSideNext))
                {
                    return current;
                }

                if (IsBox(board, leftSideNext))
                {
                    PerformMove2(board, next, dir);
                }

                if (IsBox(board, rightSideNext))
                {
                    PerformMove2(board, rightSide, dir);
                }

                if (IsEmptySpot(board, rightSideNext) && IsEmptySpot(board, leftSideNext))
                {
                    Swap(board, next, leftSideNext);
                    Swap(board, rightSide, rightSideNext);

                    return PerformMove2(board, current, dir);
                }

                return current;
            }

            if (board[current.Item1][current.Item2] == '@' && board[next.Item1][next.Item2] == '.')
            {
                Swap(board, current, next);
                return next;
            }

            return current;
        }

        public static bool IsEmptySpot(List<List<char>> board, (int, int) next)
        {
            return board[next.Item1][next.Item2] == '.';
        }

        public static bool IsWall(List<List<char>> board, (int, int) next)
        {
            return board[next.Item1][next.Item2] == '#';
        }

        public static bool IsBox(List<List<char>> board, (int, int) next)
        {
            return board[next.Item1][next.Item2] == '[' || board[next.Item1][next.Item2] == ']';
        }
    }
}