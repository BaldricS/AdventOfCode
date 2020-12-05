using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2020, 5)]
    public static class Day5_2020
    {
        public static int GetSeatNumber(string seat) => 
            seat
                .Select(c => c == 'B' || c == 'R' ? 1 : 0)
                .Aggregate(0, (acc, num) => (acc << 1) | num);


        [Solver(1)]
        public static int Solve1(IEnumerable<string> input) => input.Max(GetSeatNumber);

        [Solver(2)]
        public static int Solve2(IEnumerable<string> input)
        {
            var allSets = input.ToSortedList(GetSeatNumber);

            return allSets
                .Zip(allSets.Skip(1), (first, second) => new { first, second })
                .First(pair => pair.second - pair.first == 2)
                .first + 1;
        }
    }
}