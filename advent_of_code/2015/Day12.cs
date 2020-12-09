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

        public static long AccumulateNumbers(JsonElement element) =>
            element.ValueKind switch
            {
                JsonValueKind.Array => element.EnumerateArray().Sum(AccumulateNumbers),
                JsonValueKind.Object => element.EnumerateObject().Sum(p => AccumulateNumbers(p.Value)),
                JsonValueKind.Number => element.GetInt64(),
                _ => 0
            };

        public static bool CanProcess(JsonElement element) =>
            element
                .EnumerateObject()
                .Select(p => p.Value)
                .Where(p => p.ValueKind == JsonValueKind.String)
                .All(p => p.GetString() != "red");

        public static long AccumulateNumbersIgnoreRed(JsonElement element) =>
            element.ValueKind switch
            {
                JsonValueKind.Array => element.EnumerateArray().Sum(AccumulateNumbersIgnoreRed),
                JsonValueKind.Object when CanProcess(element) => element.EnumerateObject().Sum(p => AccumulateNumbersIgnoreRed(p.Value)),
                JsonValueKind.Number => element.GetInt64(),
                _ => 0
            };

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input) =>
            AccumulateNumbers(input.First().RootElement);

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input) =>
            AccumulateNumbersIgnoreRed(input.First().RootElement);
        
    }
}
