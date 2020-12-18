using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = IEnumerable<string>;

    [AdventOfCode(2020, 18)]
    public static class Day18_2020
    {
		[MapInput]
        public static ChallengeType Map(string[] lines)
        {
            return lines;
        }

        public static long Process(string line)
        {
            var ops = new Stack<char>();
            var output = new List<char>();

            for (int i = 0; i < line.Length; ++i)
            {
                char c = line[i];
                if (c >= '0' && c <= '9')
                {
                    output.Add(c);
                }
                else if (c == '+' || c == '*')
                {
                    while (ops.Any())
                    {
                        var o = ops.Pop();
                        if (o == '(')
                        {
                            ops.Push(o);
                            break;
                        }

                        output.Add(o);
                    }

                    ops.Push(c);
                }
                else if (c == '(')
                {
                    ops.Push(c);
                }
                else if (c == ')')
                {
                    while (ops.Any())
                    {
                        var o = ops.Pop();
                        if (o == '(')
                        {
                            break;
                        }

                        output.Add(o);
                    }
                }
            }

            while (ops.Any())
            {
                output.Add(ops.Pop());
            }

            var values = new Stack<long>();
            foreach (var c in output)
            {
                if (c >= '0' && c <= '9')
                {
                    values.Push(c - '0');
                }
                else
                {
                    long left = values.Pop();
                    long right = values.Pop();

                    long result = c switch
                    {
                        '+' => left + right,
                        _ => left * right,
                    };

                    values.Push(result);
                }
            }

            return values.Pop();
        }

        public static long Process2(string line)
        {
            var ops = new Stack<char>();
            var output = new List<char>();

            for (int i = 0; i < line.Length; ++i)
            {
                char c = line[i];
                if (c >= '0' && c <= '9')
                {
                    output.Add(c);
                }
                else if (c == '+' || c == '*')
                {
                    if (c == '*' && ops.Any() && ops.Peek() == '+')
                    {
                        while (ops.Any())
                        {
                            var o = ops.Pop();
                            if (o == '(')
                            {
                                ops.Push(o);
                                break;
                            }

                            output.Add(o);
                        }
                    }

                    ops.Push(c);
                }
                else if (c == '(')
                {
                    ops.Push(c);
                }
                else if (c == ')')
                {
                    while (ops.Any())
                    {
                        var o = ops.Pop();
                        if (o == '(')
                        {
                            break;
                        }

                        output.Add(o);
                    }
                }
            }

            while (ops.Any())
            {
                output.Add(ops.Pop());
            }

            var values = new Stack<long>();
            foreach (var c in output)
            {
                if (c >= '0' && c <= '9')
                {
                    values.Push(c - '0');
                }
                else
                {
                    long left = values.Pop();
                    long right = values.Pop();

                    long result = c switch
                    {
                        '+' => left + right,
                        _ => left * right,
                    };

                    values.Push(result);
                }
            }

            return values.Pop();
        }

        [Solver(1)]
        public static long Solve1(ChallengeType input) => input.Select(Process).Sum();

        [Solver(2)]
        public static long Solve2(ChallengeType input) => input.Select(Process2).Sum();
    }
}
