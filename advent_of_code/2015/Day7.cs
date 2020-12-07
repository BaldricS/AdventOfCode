using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    using ChallengeType = WireCommand;

    public class WireCommand
    {
        public string[] Actions;
        public string Wire;
    }

    [AdventOfCode(2015, 7)]
    public static class Day7_2015
    {
        [MapInput]
        public static IEnumerable<ChallengeType> Map(string[] lines) =>
            lines.Select(l =>
            {
                var pieces = l.Split(" -> ");
                return new WireCommand
                {
                    Actions = pieces[0].Split(' '),
                    Wire = pieces[1]
                };
            });

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input)
        {
            var values = new Dictionary<string, ushort>();

            ushort? GetValue(string val)
            {
                if (ushort.TryParse(val, out ushort i))
                {
                    return i;
                }
                else if (values.ContainsKey(val))
                {
                    return values[val];
                }

                return null;
            }

            ushort? GetUnaryValue(string val, Func<ushort, ushort> op)
            {
                var v = GetValue(val);
                return v.HasValue ? op(v.Value) : null;
            }

            ushort? GetBinaryValue(string val1, string val2, Func<ushort, ushort, ushort> op)
            {
                var first = GetValue(val1);
                var second = GetValue(val2);
                var canApply = first.HasValue && second.HasValue;

                return canApply ? op(first.Value, second.Value) : null;
            }

            Func<ushort, ushort, ushort> GetOp(string action) =>
                action switch
                {
                    "OR" => (v1, v2) => (ushort)(v1 | v2),
                    "AND" => (v1, v2) => (ushort)(v1 & v2),
                    "LSHIFT" => (v1, v2) => (ushort)(v1 << v2),
                    _ => (v1, v2) => (ushort)(v1 >> v2)
                };

            bool RunSimulation(string[] action, string wire)
            {
                var value = action.Length switch
                {
                    1 => GetValue(action[0]),
                    2 => GetUnaryValue(action[1], x => (ushort)~x),
                    3 => GetBinaryValue(action[0], action[2], GetOp(action[1])),
                    _ => null
                };

                if (value.HasValue)
                {
                    values[wire] = value.Value;
                }

                return value.HasValue;
            }

            while (input.Any())
            {
                input = input.Where(ac => !RunSimulation(ac.Actions, ac.Wire)).ToArray();
            }

            return values["a"];
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input)
        {
            var aValue = Solve1(input);

            var newInputs = input.ToArray();
            var wireB = newInputs.First(ac => ac.Wire == "b");
            wireB.Actions = new[] { $"{aValue}" };

            return Solve1(newInputs);
        }
    }
}
