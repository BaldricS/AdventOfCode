using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AOC
{
    public record Field(string Name, Pair First, Pair Second);
    public record TicketData(int[] YourTicket, int[][] OtherTickets, Field[] Fields);

    [AdventOfCode(2020, 16)]
    public static class Day16_2020
    {
        public static int[] MakeTicket(string l) => l.Split(",").Select(int.Parse).ToArray();

        public static Field MakeField(Match m) => new Field(m.Get(1), m.GetPair(2), m.GetPair(4));

		[MapInput]
        public static TicketData Map(string[] lines)
        {
            bool inFields = true;
            bool inMyTicket = false;

            var myTicket = Array.Empty<int>();
            List<Field> fields = new List<Field>();
            List<int[]> otherTickets = new List<int[]>();

            foreach (var line in lines)
            {
                if (line.Length == 0)
                {
                    continue;
                }
                else if (line == "your ticket:")
                {
                    inFields = false;
                    inMyTicket = true;

                    continue;
                }
                else if (line == "nearby tickets:")
                {
                    inMyTicket = false;

                    continue;
                }

                if (!inFields && !inMyTicket)
                {
                    otherTickets.Add(MakeTicket(line));
                }
                else if (inMyTicket)
                {
                    myTicket = MakeTicket(line);
                }
                else if (inFields)
                {
                    fields.Add(MakeField(Regex.Match(line, @"(.*): (\d+)-(\d+) or (\d+)-(\d+)")));
                }
            }

            return new TicketData(myTicket, otherTickets.ToArray(), fields.ToArray());
        }

        public static bool InRange(int n, Pair p) => n >= p.X & n <= p.Y;

        public static bool TicketFieldInRange(int n, Field field) =>
            InRange(n, field.First) || InRange(n, field.Second);

        public static List<int> GetInvalidFields(int[] ticket, Field[] fields)
        {
            List<int> invalid = new List<int>();

            foreach (var number in ticket)
            {
                if (!fields.Any(f => TicketFieldInRange(number, f)))
                {
                    invalid.Add(number);
                }
            }

            return invalid;
        }

        public static HashSet<Field> GetMatchingFields(int n, Field[] fields) =>
            fields.Where(f => TicketFieldInRange(n, f)).ToHashSet();

        public static bool IsValidTicket(int[] ticket, Field[] fields) =>
            GetInvalidFields(ticket, fields).Count == 0;

        [Solver(1)]
        public static long Solve1(TicketData input) =>
            input.OtherTickets.SelectMany(t => GetInvalidFields(t, input.Fields)).Sum();

        [Solver(2)]
        public static long Solve2(TicketData input)
        {
            var decodedFields = input.YourTicket.Select(_ => new HashSet<Field>(input.Fields)).ToArray();
            var validTickets = input.OtherTickets.Where(t => IsValidTicket(t, input.Fields)).ToArray();

            foreach (var ticket in validTickets)
            {
                for (int i = 0; i < ticket.Length; ++i)
                {
                    decodedFields[i].IntersectWith(GetMatchingFields(ticket[i], input.Fields));
                }
            }

            while (decodedFields.Any(f => f.Count > 1))
            {
                foreach (var field in decodedFields)
                {
                    if (field.Count == 1)
                    {
                        foreach (var other in decodedFields)
                        {
                            if (field == other)
                            {
                                continue;
                            }

                            other.Remove(field.First());
                        }
                    }
                }
            }

            long prod = 1;
            for (int i = 0; i < decodedFields.Length; ++i)
            {
                Console.WriteLine(decodedFields[i].Count);
                if (decodedFields[i].First().Name.StartsWith("departure"))
                {
                    prod *= input.YourTicket[i];
                }
            }

            return prod;
        }
    }
}
