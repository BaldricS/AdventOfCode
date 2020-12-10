using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = String;

    [AdventOfCode(2015, 11)]
    public static class Day11_2015
    {
        static bool HasIncrementingSection(string pw)
        {
            for (int i = 0; i < pw.Length - 2; ++i)
            {
                if (pw[i] + 1 == pw[i + 1] && pw[i + 1] + 1 == pw[i + 2])
                {
                    return true;
                }
            }

            return false;
        }

        static bool NoEvilLetters(string pw) => !Regex.IsMatch(pw, @"[iol]");

        static bool TwoDoublePairs(string pw) =>
            pw
                .Pair()
                .Where(p => p.First == p.Second)
                .Count() >= 2;

        static bool IsValidPassword(string line) =>
            HasIncrementingSection(line)
            && NoEvilLetters(line)
            && TwoDoublePairs(line);

        static string IncrementPassword(string pw)
        {
            var password = new char[8];

            for (int i = pw.Length - 1; i >= 0; --i)
            {
                char next = (char)(pw[i] + 1);
                bool hasCarry = next > 'z';

                password[i] = hasCarry ? 'a' : next;

                if (!hasCarry)
                {
                    for (int j = i - 1; j >= 0; --j)
                    {
                        password[j] = pw[j];
                    }

                    break;
                }
            }

            return string.Join("", password);
        }

        public static string NextPassword(string input)
        {
            do
            {
                input = IncrementPassword(input);
            } while (!IsValidPassword(input));

            return input;
        }


        [Solver(1)]
        public static string Solve1(ChallengeType input) => NextPassword(input);

        [Solver(2)]
        public static string Solve2(ChallengeType input) => NextPassword(NextPassword(input));
    }
}
