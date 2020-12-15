using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2020, 15)]
    public static class Day15_2020
    {
        [MapInput]
        public static IEnumerable<int> Map(string[] lines) => lines.SelectMany(l => l.Split(",").Select(int.Parse));

        [Solver(1)]
        public static long Solve1(IEnumerable<int> input)
        {
            var lastSpoken = input.Last();

            var turnsSpoken = input
                .Select((input, i) => (input, i + 1))
                .ToDictionary(kvp => kvp.input, kvp => new List<int> { kvp.Item2 });

            for (int i = input.Count() + 1; i <= 2020; ++i)
            {
                if (!turnsSpoken.ContainsKey(lastSpoken))
                {
                    turnsSpoken[lastSpoken] = new List<int>();
                    turnsSpoken[lastSpoken].Insert(0, i);

                    lastSpoken = 0;
                }
                else if(turnsSpoken[lastSpoken].Count == 1)
                {
                    lastSpoken = 0;

                    var l = turnsSpoken.GetValueOrDefault(lastSpoken, new List<int>());
                    l.Insert(0, i);
                    turnsSpoken[lastSpoken] = l;
                }
                else
                {
                    lastSpoken = turnsSpoken[lastSpoken][0] - turnsSpoken[lastSpoken][1];

                    var l = turnsSpoken.GetValueOrDefault(lastSpoken, new List<int>());
                    l.Insert(0, i);
                    turnsSpoken[lastSpoken] = l;
                }
            }

            return lastSpoken;
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<int> input)
        {
            var lastSpoken = input.Last();

            var turnsSpoken = input
                .Select((input, i) => (input, i + 1))
                .ToDictionary(kvp => kvp.input, kvp => new int[] { kvp.Item2, -1});

            for (int i = input.Count() + 1; i <= 30000000; ++i)
            {
                if (!turnsSpoken.ContainsKey(lastSpoken))
                {
                    turnsSpoken[lastSpoken] = new int[] { i, -1 };

                    lastSpoken = 0;
                }
                else if(turnsSpoken[lastSpoken][1] == -1)
                {
                    lastSpoken = 0;

                    var arr = turnsSpoken.GetValueOrDefault(lastSpoken, new int[] { -1, -1 });
                    arr[1] = arr[0];
                    arr[0] = i;

                    turnsSpoken[lastSpoken] = arr;
                }
                else
                {
                    lastSpoken = turnsSpoken[lastSpoken][0] - turnsSpoken[lastSpoken][1];

                    var arr = turnsSpoken.GetValueOrDefault(lastSpoken, new int[] { -1, -1 });
                    arr[1] = arr[0];
                    arr[0] = i;

                    turnsSpoken[lastSpoken] = arr;
                }
            }

            return lastSpoken;
        }
    }
}
