using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static bool IsValidInt(string maybeInt, int min, int max)
        {
            if (!int.TryParse(maybeInt, out int val))
            {
                return false;
            }

            return val >= min && val <= max;
        }

        public static bool IsValidPassportStrict(string passport)
        {
            if (!IsValidPassport(passport))
            {
                return false;
            }

            var values = passport.Split(" ").ToDictionary(k => k.Split(":")[0], k => k.Split(":")[1]);
            
            if (!IsValidInt(values["byr"], 1920, 2020))
            {
                return false;
            }
            
            if (!IsValidInt(values["iyr"], 2010, 2020))
            {
                return false;
            }

            if (!IsValidInt(values["eyr"], 2020, 2030))
            {
                return false;
            }

            var heightRegex = new Regex(@"^(\d+)(cm|in)$");
            var matches = heightRegex.Match(values["hgt"]);
            if (matches.Groups[2].Value == "in")
            {
                if (!IsValidInt(matches.Groups[1].Value, 59, 76))
                {
                    return false;
                }
            }
            else if (matches.Groups[2].Value == "cm")
            {
                if (!IsValidInt(matches.Groups[1].Value, 150, 193))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


            var hclRegex = new Regex(@"^#[0-9a-f]{6}$", RegexOptions.IgnoreCase);
            if (!hclRegex.IsMatch(values["hcl"]))
            {
                return false;
            }

            var legalEyeColors = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            if (!legalEyeColors.Any(e => values["ecl"].Equals(e)))
            {
                return false;
            }

            var idRegex = new Regex(@"^\d{9}$");
            if (!idRegex.IsMatch(values["pid"]))
            {
                return false;
            }

            return true;
        }

        public static int Solve1(string[] lines, Func<string, bool> policy)
        {
            var passportPieces = new List<string>();
            int passports = 0;

            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Length == 0)
                {
                    if (policy(string.Join(" ", passportPieces)))
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
            var puzzle1 = new AdventOfCode(4, 1);
            puzzle1.Run(lines => Solve1(lines, IsValidPassport));

            var puzzle2 = new AdventOfCode(4, 2);
            puzzle1.Run(lines => Solve1(lines, IsValidPassportStrict));
        }
    }
}
