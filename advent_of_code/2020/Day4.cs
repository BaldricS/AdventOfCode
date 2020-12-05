using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2020, 4)]
    public static class Day4_2020
    {
        public static bool IsValidPassport(string passport)
        {
            var keys = new[] { "byr:", "iyr:", "eyr:", "hgt:", "hcl:", "ecl:", "pid:" };

            return keys.All(k => passport.Contains(k, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsValidBirthYear(string value) => value.IsInRange(1920, 2002);

        public static bool IsValidIssueYear(string value) => value.IsInRange(2010, 2020);

        public static bool IsValidExpirationYear(string value) => value.IsInRange(2020, 2030);

        public static bool IsValidHeight(string value)
        {
            var heightRegex = new Regex(@"^(\d+)(cm|in)$");
            var matches = heightRegex.Match(value);
            if (matches.Groups[2].Value == "in" && matches.Groups[1].Value.IsInRange(59, 76))
            {
                return true;
            }
            else if (matches.Groups[2].Value == "cm" && matches.Groups[1].Value.IsInRange(150, 193))
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

        [MapInput]
        public static IEnumerable<string> Map(string[] lines) =>
            lines
                .Aggregate(new List<List<string>> { new List<string>() }, (acc, line) =>
                {
                    if (line.Length == 0)
                    {
                        acc.Add(new List<string>());
                    }
                    else
                    {
                        acc.Last().Add(line);
                    }

                    return acc;
                })
                .Select(line => string.Join(" ", line));

        [Solver(1)]
        public static int Solve1(IEnumerable<string> passports) => passports.Count(IsValidPassport);

        [Solver(2)]
        public static int Solve2(IEnumerable<string> passports) => passports.Count(IsValidPassportStrict);
    }
}