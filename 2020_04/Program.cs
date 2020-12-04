using System;
using System.Collections.Generic;
using System.Linq;

using AOC;

namespace Day4
{
    class Program
    {
        public static bool IsValidPassport(string passport)
        {
            var keys = new[] { "byr:", "iyr:", "eyr:", "hgt:", "hcl:", "ecl:", "pid:" };

            return keys.All(k => passport.Contains(k, StringComparison.OrdinalIgnoreCase));
        }

        public static int Solve1(string[] lines)
        {
            var passportPieces = new List<string>();
            int passports = 0;

            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Length == 0)
                {
                    if (IsValidPassport(string.Join(" ", passportPieces)))
                    {
                        ++passports;
                    }

                    passportPieces.Clear();
                }
                else
                {
                    passportPieces.Add(lines[i]);
                }
            }

            return passports;
        }

        static void Main()
        {
            var puzzle1 = new AdventOfCode(3, 1);
            puzzle1.Run(Solve1);
        }
    }
}
