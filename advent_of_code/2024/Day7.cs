using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    public record Equation20247(long Result, long[] Operands);

    [AdventOfCode(2024, 7)]
    public static class Day7_2024
    {
        [MapInput]
        public static IEnumerable<Equation20247> Map(string[] lines) => lines
            .Select(l => l.Split(": "))
            .Select(vals => new Equation20247(long.Parse(vals[0]), vals[1].Split(' ').Select(long.Parse).ToArray()));

        [Solver(1)]
        public static long Solve1(IEnumerable<Equation20247> input) =>
            input.Where(eq => Evaluate(eq.Result, eq.Operands[0], 1, eq.Operands, [Add, Mul]))
                .Select(eq => eq.Result)
                .Sum();

        public static long Add(long a, long b) => a + b;

        public static long Mul(long a, long b) => a * b;

        public static long Concat(long a, long b) => long.Parse($"{a}{b}");

        public static bool Evaluate(long target, long a, int idx, long[] rest, Func<long, long, long>[] ops)
        {
            if (a > target)
            {
                return false;
            }

            if (idx == rest.Length)
            {
                return target == a;
            }

            long b = rest[idx++];
            return ops.Any(op => Evaluate(target, op(a, b), idx, rest, ops));
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<Equation20247> input) =>
            input.Where(eq => Evaluate(eq.Result, eq.Operands[0], 1, eq.Operands, [Add, Mul, Concat]))
                .Select(eq => eq.Result)
                .Sum();
    }
}