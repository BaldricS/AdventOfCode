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
                return FindMidpoint(seat.Substring(1), low + mid + 1, high, highC);
            }

            return FindMidpoint(seat.Substring(1), low, low + mid, highC);
        }

        public static int GetSeatNumber(string line)
        {
            var rowStr = line.Substring(0, 7);
            var colStr = line.Substring(7);

            return FindMidpoint(rowStr, 0, 127, 'B') * 8 + FindMidpoint(colStr, 0, 7, 'R');
        }

        [Solver(1)]
        public static int Solve1(string[] input) => input.Select(GetSeatNumber).Max();

        [Solver(2)]
        public static int Solve2(string[] input)
        {
            var allSeats = input.Select(GetSeatNumber).ToArray();
            Array.Sort(allSeats);

            for (int i = 1; i < allSeats.Length; ++i)
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