using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    public record Password(int Low, int High, char Letter, string Cleartext);

    [AdventOfCode(2020, 2)]
    public static class Day2_2020
    {
        static bool IsValidPasswordPolicy1(Password password) =>
            password.Cleartext
                .Count(c => c == password.Letter)
                .IsInRange(password.Low, password.High);

        static bool IsValidPasswordPolicy2(Password password)
        {
            var index1 = password.Low - 1;
            var index2 = password.High - 1;
            var passLength = password.Cleartext.Length;
            if (passLength <= index1 || passLength <= index2)
            {
                return false;
            }

            var letter1 = password.Cleartext[index1];
            var letter2 = password.Cleartext[index2];

            return (letter1 == password.Letter || letter2 == password.Letter) && letter1 != letter2;
        }

        [MapInput]
        public static IEnumerable<Password> Map(string[] lines)
        {
            var regex = new Regex(@"(\d+)-(\d+) ([a-z]): (.*)$", RegexOptions.IgnoreCase);

            Password MapLine(string line)
            {
                var match = regex.Match(line);

                return new Password(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    match.Groups[3].Value[0],
                    match.Groups[4].Value
                );
            }

            return lines.Select(MapLine);
        }

        [Solver(1)]
        public static int Solve1(IEnumerable<Password> passwords) => passwords.Count(IsValidPasswordPolicy1);

        [Solver(2)]
        public static int Solve2(IEnumerable<Password> passwords) => passwords.Count(IsValidPasswordPolicy2);
    }
}
