using System.Collections.Generic;
using System.Linq;

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

        [Solver(1)]
        public static long Solve1(ChallengeType input)
        {
            var c = new Calculator();

            c.AddOp('+', (a, b) => a + b);
            c.AddOp('*', (a, b) => a * b);

            return input.Select(l => c.Evaluate(l)).Sum();
        }

        [Solver(2)]
        public static long Solve2(ChallengeType input)
        {
            var c = new Calculator();

            c.AddOp('+', (a, b) => a + b, 1);
            c.AddOp('*', (a, b) => a * b);

            return input.Select(l => c.Evaluate(l)).Sum();
        }
    }
}
