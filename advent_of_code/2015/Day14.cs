using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Reindeer;

    public record Reindeer(string Name, int Flight, int FlightTime, int RestTime);
    public record State(int Distance, int FlightTime, int RestTime, bool Flying)
    {
        public bool Resting { get => !Flying; }
    }

    [AdventOfCode(2015, 14)]
    public static class Day14_2015
    {
        public static Reindeer ToData(Match m) =>
            new Reindeer(m.Get(1), m.Get(2).AsInt(), m.Get(3).AsInt(), m.Get(4).AsInt());

        [MapInput]
        public static IEnumerable<ChallengeType> Map(string[] lines) =>
            lines.Select(l => Regex.Match(l, @"^(\w+) .* (\d+) .* (\d+) .* (\d+)"))
                .Select(ToData);

        public static State Tick(State s, Reindeer r)
        {
            if (s.Flying && s.FlightTime > 0)
            {
                return s with { Distance = s.Distance + r.Flight, FlightTime = s.FlightTime - 1 };
            }
            else if (s.Flying && s.FlightTime == 0)
            {
                return s with { Flying = false, RestTime = r.RestTime - 1 };
            }
            else if (s.Resting && s.RestTime > 0)
            {
                return s with { RestTime = s.RestTime - 1 };
            }
            else
            {
                return s with { Distance = s.Distance + r.Flight, Flying = true, FlightTime = r.FlightTime - 1 };
            }
        }

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input)
        {
            var reindeer = input.ToArray();
            var states = reindeer.Select(r => new State(0, r.FlightTime, 0, true)).ToArray();

            for (int i = 0; i < 2503; ++i)
            {
                for (int s = 0; s < states.Length; ++s)
                {
                    states[s] = Tick(states[s], reindeer[s]);
                }
            }

            return states.Max(s => s.Distance);
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input)
        {
            var reindeer = input.ToArray();
            var states = reindeer.Select(r => new State(0, r.FlightTime, 0, true)).ToArray();
            var points = reindeer.Select(_ => 0).ToArray();

            for (int i = 0; i < 2503; ++i)
            {
                for (int s = 0; s < states.Length; ++s)
                {
                    states[s] = Tick(states[s], reindeer[s]);
                }

                int furthest = states.Max(s => s.Distance);

                for (int s = 0; s < states.Length; ++s)
                {
                    if (states[s].Distance == furthest)
                    {
                        ++points[s];
                    }
                }
            }

            return points.Max();
        }
    }
}
