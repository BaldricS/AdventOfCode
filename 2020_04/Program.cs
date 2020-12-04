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

        public static bool IsValidBirthYear(string value) => IsValidInt(value, 1920, 2002);

        public static bool IsValidIssueYear(string value) => IsValidInt(value, 2010, 2020);

        public static bool IsValidExpirationYear(string value) => IsValidInt(value, 2020, 2030);

        public static bool IsValidHeight(string value)
        {
            var heightRegex = new Regex(@"^(\d+)(cm|in)$");
            var matches = heightRegex.Match(value);
            if (matches.Groups[2].Value == "in" && IsValidInt(matches.Groups[1].Value, 59, 76))
            {
                return true;
            }
            else if (matches.Groups[2].Value == "cm" && IsValidInt(matches.Groups[1].Value, 150, 193))
            {
                return true;
            }

            return false;
        }

        public static bool IsValidHairColor(string value) => new Regex(@"^#[0-9a-f]{6}$", RegexOptions.IgnoreCase).IsMatch(value);

        public static bool IsValidEyeColor(string value) =>
            new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Any(e => value.Equals(e));

        public static bool IsValidPin(string value) => new Regex(@"^\d{9}$").IsMatch(value);

        public static bool IsValidPassportStrict(string passport)
        {
            if (!IsValidPassport(passport))
            {
                return false;
            }

            var values = passport.Split(" ").Select(k => k.Split(":")).ToDictionary(k => k[0], k => k[1]);

            return IsValidBirthYear(values["byr"])
                && IsValidIssueYear(values["iyr"])
                && IsValidExpirationYear(values["eyr"])
                && IsValidHeight(values["hgt"])
                && IsValidHairColor(values["hcl"])
                && IsValidEyeColor(values["ecl"])
                && IsValidPin(values["pid"]);
        }

        public static int Solve(string[] lines, Func<string, bool> policy)
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
            puzzle1.Run(lines => Solve(lines, IsValidPassport));

            var puzzle2 = new AdventOfCode(4, 2);
            puzzle1.Run(lines => Solve(lines, IsValidPassportStrict));
        }
    }
}
