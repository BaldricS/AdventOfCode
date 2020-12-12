using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = ShipAction;

    public record ShipAction(char Action, int Amount);

    [AdventOfCode(2020, 12)]
    public static class Day12_2020
    {
        public static ShipAction ToData(Match m) =>
            new ShipAction(m.Get(1)[0], m.Get(2).AsInt());

        [MapInput]
        public static IEnumerable<ChallengeType> Map(string[] lines) =>
            lines.Select(l => Regex.Match(l, @"(\w)(\d+)")).Select(ToData);

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input)
        {
            var pos = new Pair(0, 0);
            var directions = new[] { new Pair(1, 0), new Pair(0, -1), new Pair(-1, 0), new Pair(0, 1) };

            var facing = 0;

            foreach (var action in input)
            {
                var move = action.Action switch
                {
                    'E' => 0,
                    'S' => 1,
                    'W' => 2,
                    'N' => 3,
                    'F' => facing,
                    _ => -1
                };

                if (move > -1)
                {
                    var dir = directions[move];
                    pos = pos with { X = pos.X + dir.X * action.Amount, Y = pos.Y + dir.Y * action.Amount };
                }
                else
                {
                    var rot = action.Action switch {
                        'R' => action.Amount,
                        'L' => 360 - action.Amount,
                        _ => 0
                    };

                    facing = (facing + rot / 90) % 4;
                }
            }

            return Math.Abs(pos.X) + Math.Abs(pos.Y);
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input)
        {
            var pos = new Pair(0, 0);
            var wpPos = new Pair(10, 1);
            var directions = new[] { new Pair(1, 0), new Pair(0, -1), new Pair(-1, 0), new Pair(0, 1) };

            foreach (var action in input)
            {
                if (action.Action == 'F')
                {
                    pos = pos with { X = pos.X + wpPos.X * action.Amount, Y = pos.Y + wpPos.Y * action.Amount };
                    continue;
                }

                var move = action.Action switch
                {
                    'E' => 0,
                    'S' => 1,
                    'W' => 2,
                    'N' => 3,
                    _ => -1
                };

                if (move > -1)
                {
                    var dir = directions[move];
                    wpPos = wpPos with { X = wpPos.X + dir.X * action.Amount, Y = wpPos.Y + dir.Y * action.Amount };
                }
                else
                {
                    var rot = (Math.PI / 180.0) * action.Action switch {
                        'R' => action.Amount,
                        'L' => 360 - action.Amount,
                        _ => 0
                    };

                    var xRot = (int)Math.Cos(rot);
                    var yRot = (int)Math.Sin(rot);

                    wpPos = wpPos with { X = wpPos.X * xRot + wpPos.Y * yRot, Y = wpPos.Y * xRot - wpPos.X * yRot };
                }
            }

            return Math.Abs(pos.X) + Math.Abs(pos.Y);
        }
    }
}
