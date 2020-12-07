using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

            bool TryApply(string val1, string val2, string wire, Func<ushort, ushort, ushort> op)
            {
                var firstIsInt = ushort.TryParse(val1, out ushort first);
                var secondIsInt = ushort.TryParse(val2, out ushort second);

                if (firstIsInt && secondIsInt)
                {
                    values[wire] = op(first, second);
                    return true;
                } else if (firstIsInt && !secondIsInt && values.ContainsKey(val2))
                {
                    values[wire] = op(first, values[val2]);
                    return true;
                }
                else if (secondIsInt && !firstIsInt && values.ContainsKey(val1))
                {
                    values[wire] = op(values[val1], second);
                    return true;
                }
                else if (values.ContainsKey(val1) && values.ContainsKey(val2))
                {
                    values[wire] = op(values[val1], values[val2]);
                    return true;
                }

                return false;
            }

            bool ProcessAction(string[] action, string wire)
            {
                if (action.Length == 1)
                {
                    if (int.TryParse(action[0], out int val))
                    {
                        values.Add(wire, action[0].AsUshort());
                        return true;
                    }
                    else if (values.ContainsKey(action[0]))
                    {
                        values.Add(wire, values[action[0]]);
                        return true;
                    }

                    return false;
                }
                if (action.Length == 2)
                {
                    if (values.ContainsKey(action[1]))
                    {
                        values.Add(wire, (ushort)~values[action[1]]);
                        return true;
                    }

                    return false;
                }
                else if (action.Length == 3)
                {
                    switch (action[1])
                    {
                        case "OR":
                            return TryApply(action[0], action[2], wire, (v1, v2) => (ushort)(v1 | v2));

                        case "AND":
                            return TryApply(action[0], action[2], wire, (v1, v2) => (ushort)(v1 & v2));

                        case "LSHIFT":
                            return TryApply(action[0], action[2], wire, (v1, v2) => (ushort)(v1 << v2));

                        case "RSHIFT":
                            return TryApply(action[0], action[2], wire, (v1, v2) => (ushort)(v1 >> v2));
                    }
                }

                return false;
            }

            var process = true;
            while(process)
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
            var newInputs = input.ToList();
            var wireB = newInputs.Find(ac => ac.Wire == "b");
            wireB.Actions = new[] { $"{aValue}" };

            return Solve1(newInputs);
        }
    }
}
