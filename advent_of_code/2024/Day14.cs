using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    public record Input202414(((int, int) Position, (int, int) Velocity)[] Robots);

    [AdventOfCode(2024, 14)]
    public static class Day14_2024
    {
        [MapInput]
        public static Input202414 Map(string[] lines)
        {
            var r = new Regex(@"(-?\d+)");
            var numbers = lines.SelectMany(l => r.Matches(l).Select(m => m.Value)).Select(int.Parse).ToList();
            var list = new List<((int, int), (int, int))>();
            for (int i = 0; i < numbers.Count; i += 4)
            {
                list.Add(
                    (
                        (numbers[i], numbers[i + 1]),
                        (numbers[i + 2], numbers[i + 3])
                    )
                );
            }

            return new Input202414([.. list]);
        }

        public static (int, int) Move((int, int) pos, (int, int) v, int wide, int tall)
        {
            (int, int) newPos = (pos.Item1 + v.Item1, pos.Item2 + v.Item2);
            (int, int) modified = ((newPos.Item1 + wide) % wide, (newPos.Item2 + tall) % tall);

            return modified;
        }

        public static int Quadrant((int, int) pos, int wide, int tall)
        {
            int mWide = wide / 2;
            int mTall = tall / 2;

            if (pos.Item1 < mWide && pos.Item2 < mTall)
            {
                return 1;
            }

            if (pos.Item1 > mWide && pos.Item2 < mTall)
            {
                return 2;
            }

            if (pos.Item1 < mWide && pos.Item2 > mTall)
            {
                return 3;
            }

            if (pos.Item1 > mWide && pos.Item2 > mTall)
            {
                return 4;
            }

            return -1;
        }

        [Solver(1)]
        public static int Solve1(Input202414 input)
        {
            int wide = 101;
            int tall = 103;
            int totalMap = wide * tall;
            for (int i = 0; i < 100; ++i)
            {
                for (int r = 0; r < input.Robots.Length; ++r)
                {
                    var robot = input.Robots[r];
                    input.Robots[r].Position = Move(robot.Position, robot.Velocity, wide, tall);
                }
            }

            return input.Robots.Select(r => Quadrant(r.Position, wide, tall)).Where(q => q != -1).GroupBy(q => q).Aggregate(1, (a, b) => a * b.Count());
        }

        [Solver(2)]
        public static int Solve2(Input202414 input)
        {
            int wide = 101;
            int tall = 103;
            int totalMap = wide * tall;
            for (int i = 0; i < 100000; ++i)
            {
                for (int r = 0; r < input.Robots.Length; ++r)
                {
                    var robot = input.Robots[r];
                    input.Robots[r].Position = Move(robot.Position, robot.Velocity, wide, tall);
                }

                var positions = input.Robots
                    .Select(r => r.Position)
                    .GroupBy(r => r.Item1)
                    .Select(r => r.OrderBy(i => i.Item2).Zip(r.OrderBy(i => i.Item2).Skip(1)).Select(i => i.Second.Item2 - i.First.Item2).Where(i => i == 1).Count())
                    .OrderBy(r => r)
                    .Reverse()
                    .First();

                if (positions > 15)
                {
                    return i + 1;
                    /*
                    var hashed = input.Robots.Select(r => r.Position).ToHashSet();

                    for (int t = 0; t < tall; ++t)
                    {
                        for (int w = 0; w < wide; ++w)
                        {
                            if (hashed.Contains((w, t)))
                            {
                                Console.Write("R");
                            }
                            else
                            {
                                Console.Write(".");
                            }
                        }
                        Console.WriteLine();
                    }
                */
                }

            }

            return -1;
        }
    }
}