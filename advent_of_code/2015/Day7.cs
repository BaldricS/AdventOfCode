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
            var commands = new Dictionary<string, Func<ushort, ushort, ushort>>
            {
                ["OR"] = (v1, v2) => (ushort)(v1 | v2),
                ["AND"] = (v1, v2) => (ushort)(v1 & v2),
                ["LSHIFT"] = (v1, v2) => (ushort)(v1 << v2),
                ["RSHIFT"] = (v1, v2) => (ushort)(v1 >> v2)
            };

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

            bool TryApply(string val1, string val2, string wire, Func<ushort, ushort, ushort> op)
            {
                var first = GetValue(val1);
                var second = GetValue(val2);

                if (first.HasValue && second.HasValue)
                {
                    values[wire] = op(first.Value, second.Value);
                    return true;
                }

                return false;
            }

            bool TryApplySingle(string val, string wire, Func<ushort, ushort> op)
            {
                var value = GetValue(val);
                if (value.HasValue)
                {
                    values[wire] = op(value.Value);
                    return true;
                }

                return false;
            }

            bool ProcessAction(string[] action, string wire) =>
                action.Length switch
                {
                    1 => TryApplySingle(action[0], wire, x => x),
                    2 => TryApplySingle(action[1], wire, x => (ushort)~x),
                    3 => TryApply(action[0], action[2], wire, commands[action[1]]),
                    _ => false,
                };

            var process = true;
            while (process)
            {
                var remaining = input.Where(ac => !ProcessAction(ac.Actions, ac.Wire)).ToArray();
                process = remaining.Length > 0;
                input = remaining;
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
