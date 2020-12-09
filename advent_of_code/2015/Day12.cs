using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AOC
{
    using ChallengeType = JsonDocument;

    [AdventOfCode(2015, 12)]
    public static class Day12_2015
    {
        [MapInput]
        public static IEnumerable<ChallengeType> Map(string[] lines) =>
            lines.Select(l => ChallengeType.Parse(l));

        public static long Accumulate(JsonElement ele, Func<JsonElement, bool> pred) =>
            ele.ValueKind switch
            {
                JsonValueKind.Object when pred(ele) => ele.EnumerateObject().Sum(p => Accumulate(p.Value, pred)),
                JsonValueKind.Array => ele.EnumerateArray().Sum(e => Accumulate(e, pred)),
                JsonValueKind.Number => ele.GetInt64(),
                _ => 0
            };

        public static bool HasNoRedValue(JsonElement element) =>
            element
                .EnumerateObject()
                .Select(p => p.Value)
                .Where(p => p.ValueKind == JsonValueKind.String)
                .All(p => p.GetString() != "red");

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input) =>
            Accumulate(input.First().RootElement, obj => true);

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input) =>
            Accumulate(input.First().RootElement, HasNoRedValue);

    }
}
