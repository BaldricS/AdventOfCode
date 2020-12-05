using System;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2020, 5)]
    public static class Day5_2020
    {
        public static int FindMidpoint(string seat, int low, int high, char highC)
        {
            if (seat.Length == 1)
            {
                return seat[0] == highC ? high : low;
            }

            var mid = (high - low) / 2;
            if (seat[0] == highC)
            {
                return FindMidpoint(seat[1..], low + mid + 1, high, highC);
            }

            return FindMidpoint(seat[1..], low, low + mid, highC);
        }

        public static int GetSeatNumber(string line) =>
            FindMidpoint(line[..7], 0, 127, 'B') * 8 + FindMidpoint(line[7..], 0, 7, 'R');

        [Solver(1)]
        public static int Solve1(string[] input) => input.Max(GetSeatNumber);

        [Solver(2)]
        public static int Solve2(string[] input)
        {
            var allSeats = input.ToSortedList(GetSeatNumber);

            for (int i = 1; i < allSeats.Count; ++i)
            {
                int prev = allSeats[i - 1];
                int curr = allSeats[i];

                if ((curr - prev) == 2)
                {
                    return curr - 1;
                }
            }

            return -1;
        }
    }
}