using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    public struct Password
    {
        public int low;
        public int high;
        public char letter;
        public string password;
    }

    [AdventOfCode(2020, 2)]
    public static class Day2_2020
    {
        static bool IsValidPasswordPolicy1(Password password) =>
            password.password
                .Count(c => c == password.letter)
                .IsInRange(password.low, password.high);

        static bool IsValidPasswordPolicy2(Password password)
        {
            var index1 = password.low - 1;
            var index2 = password.high - 1;
            var passLength = password.password.Length;
            if (passLength <= index1 || passLength <= index2)
            {
                return false;
            }

            var letter1 = password.password[index1];
            var letter2 = password.password[index2];

            return (letter1 == password.letter || letter2 == password.letter) && letter1 != letter2;
        }

        [MapInput]
        public static IEnumerable<Password> Map(string[] lines)
        {
            var regex = new Regex(@"(\d+)-(\d+) ([a-z]): (.*)$", RegexOptions.IgnoreCase);

            Password MapLine(string line)
            {
                var match = regex.Match(line);

                return new Password
                {
                    low = int.Parse(match.Groups[1].Value),
                    high = int.Parse(match.Groups[2].Value),
                    letter = match.Groups[3].Value[0],
                    password = match.Groups[4].Value
                };
            }

            return lines.Select(MapLine);
        }

        [Solver(1)]
        public static int Solve1(IEnumerable<Password> passwords) => passwords.Count(IsValidPasswordPolicy1);

        [Solver(2)]
        public static int Solve2(IEnumerable<Password> passwords) => passwords.Count(IsValidPasswordPolicy2);
    }
}
