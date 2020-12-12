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

        private static Pair Update(Pair pos, Pair dir, int amount) =>
            pos with { X = pos.X + dir.X * amount, Y = pos.Y + dir.Y * amount };

        public static long DriveTheBoat(IEnumerable<ChallengeType> input, bool isMobileWaypoint)
        {
            var pos = new Pair(0, 0);
            var wpPos = new Pair(1, 0);

            foreach (var action in input)
            {
                var dir = action.Action switch
                {
                    'E' => new Pair(1, 0),
                    'S' => new Pair(0, -1),
                    'W' => new Pair(-1, 0),
                    'N' => new Pair(0, 1),
                    'F' => wpPos,
                    _ => null
                };

                if (dir != null)
                {
                    if (isMobileWaypoint && action.Action != 'F')
                    {
                        wpPos = Update(wpPos, dir, action.Amount);
                    }
                    else
                    {
                        pos = Update(pos, dir, action.Amount);
                    }
                }
                else
                {
                    var rot = (Math.PI / 180.0) * action.Action switch {
                        'R' => 360 - action.Amount,
                        'L' => action.Amount,
                        _ => 0
                    };

                    var xRot = (int)Math.Cos(rot);
                    var yRot = (int)Math.Sin(rot);

                    wpPos = wpPos with { X = wpPos.X * xRot - wpPos.Y * yRot, Y = wpPos.Y * xRot + wpPos.X * yRot };
                }
            }

            return Math.Abs(pos.X) + Math.Abs(pos.Y);
        }

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input) =>
            DriveTheBoat(input, false);

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input) =>
            DriveTheBoat(input.Prepend(new ShipAction('E', 9)).Prepend(new ShipAction('N', 1)), true);
    }
}
