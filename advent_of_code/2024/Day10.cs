using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC
{
    public record Input202410(string[] Lines);

    [AdventOfCode(2024, 10)]
    public static class Day10_2024
    {
        [MapInput]
        public static Input202410 Map(string[] lines) => new(lines);

        [Solver(1)]
        public static int Solve1(Input202410 input)
        {
            return input.Lines.SelectMany((r, rIdx) => r.Select((c, cIdx) => {
                if (c != '9')
                {
                    return 0;
                }

                HashSet<(int, int)> trailEnds = [];
                Score(input.Lines, (rIdx, cIdx), ':', trailEnds);
                return trailEnds.Count;
            }))
            .Sum();
        }

        public static int Score(string[] input, (int, int) curr, char prev, HashSet<(int, int)> trailEnds)
        {
            if (curr.Item1 < 0 || curr.Item1 >= input.Length || curr.Item2 < 0 || curr.Item2 >= input[curr.Item1].Length)
            {
                return 0;
            }

            if (prev - input[curr.Item1][curr.Item2] != 1)
            {
                return 0;
            }

            if (input[curr.Item1][curr.Item2] == '0')
            {
                trailEnds.Add((curr.Item1, curr.Item2));
                return 1;
            }

            char nextPrev = input[curr.Item1][curr.Item2];
            return Score(input, (curr.Item1 + 1, curr.Item2), nextPrev, trailEnds) +
                Score(input, (curr.Item1 - 1, curr.Item2), nextPrev, trailEnds) +
                Score(input, (curr.Item1, curr.Item2 + 1), nextPrev, trailEnds) +
                Score(input, (curr.Item1, curr.Item2 - 1), nextPrev, trailEnds);
        }

        public static int Score(string[] input, (int, int) curr, char prev)
        {
            if (curr.Item1 < 0 || curr.Item1 >= input.Length || curr.Item2 < 0 || curr.Item2 >= input[curr.Item1].Length)
            {
                return 0;
            }

            if (prev - input[curr.Item1][curr.Item2] != 1)
            {
                return 0;
            }

            if (input[curr.Item1][curr.Item2] == '0')
            {
                return 1;
            }

            char nextPrev = input[curr.Item1][curr.Item2];
            return Score(input, (curr.Item1 + 1, curr.Item2), nextPrev) +
                Score(input, (curr.Item1 - 1, curr.Item2), nextPrev) +
                Score(input, (curr.Item1, curr.Item2 + 1), nextPrev) +
                Score(input, (curr.Item1, curr.Item2 - 1), nextPrev);
        }

        [Solver(2)]
        public static long Solve2(Input202410 input)
        {
            return input.Lines.SelectMany((r, rIdx) => r.Select((c, cIdx) => {
                if (c != '9')
                {
                    return 0;
                }

                return Score(input.Lines, (rIdx, cIdx), ':');
            }))
            .Sum();
        }
    }
}