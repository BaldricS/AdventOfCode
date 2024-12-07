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
            input.Where(eq => Evaluate(eq.Result, eq.Operands[0], 1, eq.Operands))
                .Select(eq => eq.Result)
                .Sum();

        public static bool Evaluate(long target, long a, int idx, long[] rest)
        {
            if (idx == rest.Length)
            {
                return target == a;
            }

            long b = rest[idx++];
            long tempAdd = a + b;
            long tempMul = a * b;

            return Evaluate(target, tempAdd, idx, rest) || Evaluate(target, tempMul, idx, rest);
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<Equation20247> input) =>
            input.Where(eq => Evaluate2(eq.Result, eq.Operands[0], 1, eq.Operands))
                .Select(eq => eq.Result)
                .Sum();

        public static bool Evaluate2(long target, long a, int idx, long[] rest)
        {
            if (idx == rest.Length)
            {
                return target == a;
            }

            long b = rest[idx++];
            long tempAdd = a + b;
            long tempMul = a * b;
            long tempConcat = long.Parse($"{a}{b}");

            return Evaluate2(target, tempAdd, idx, rest) || Evaluate2(target, tempMul, idx, rest) || Evaluate2(target, tempConcat, idx, rest);
        }
    }
}