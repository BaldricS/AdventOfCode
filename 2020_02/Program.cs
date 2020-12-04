using System;
using System.Linq;
using System.Text.RegularExpressions;

using AOC;

namespace Day2
{
    struct Password
    {
        public int low;
        public int high;
        public char letter;
        public string password;
    }

    class Program
    {
        static Password Parse(string line)
        {
            var regex = new Regex(@"(\d+)-(\d+) ([a-z]): (.*)$", RegexOptions.IgnoreCase);
            var match = regex.Match(line);

            return new Password
            {
                low = int.Parse(match.Groups[1].Value),
                high = int.Parse(match.Groups[2].Value),
                letter = match.Groups[3].Value[0],
                password = match.Groups[4].Value
            };
        }

        static bool IsValidPasswordPolicy1(Password password)
        {
            var desiredLetters = password.password.Where(c => c == password.letter).Count();

            return desiredLetters >= password.low && desiredLetters <= password.high;
        }

        static bool IsValidPasswordPolcy2(Password password)
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

        static Func<Password[], int> PasswordCounter(Func<Password, bool> matchesPolicy) =>
            passwords => passwords.Where(matchesPolicy).Count();

        static void Main()
        {
            var puzzle1 = new AdventOfCode(2, 1);
            puzzle1.Run(Parse, PasswordCounter(IsValidPasswordPolicy1));

            var puzzle2 = new AdventOfCode(2, 2);
            puzzle2.Run(Parse, PasswordCounter(IsValidPasswordPolcy2));
        }
    }
}
