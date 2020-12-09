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

        static bool TwoDoublePairs(string pw)
        {
            bool hasOnePair = false;

            for (int i = 0; i < pw.Length - 1; ++i)
            {
                if (pw[i] == pw[i + 1])
                {
                    if (hasOnePair)
                    {
                        return true;
                    }

                    hasOnePair = true;
                    ++i;
                }
            }

            return false;
        }

        static bool IsValidPassword(string line) =>
            HasIncrementingSection(line)
            && NoEvilLetters(line)
            && TwoDoublePairs(line);

        static string IncrementPassword(string pw)
        {
            var password = new char[8];
            bool hadCarry = false;

            for (int i = pw.Length - 1; i >= 0; --i)
            {
                char next = (char)(pw[i] + 1);
                if (next > 'z')
                {
                    next = 'a';
                    hadCarry = true;
                }

                password[i] = next;

                if (!hadCarry)
                {
                    for (int j = i - 1; j >= 0; --j)
                    {
                        password[j] = pw[j];
                    }

                    break;
                }

                hadCarry = false;
            }

            return string.Join("", password);
        }

        [Solver(1)]
        public static string Solve1(IEnumerable<ChallengeType> input)
        {
            var pw = input.First();
            while (!IsValidPassword(pw))
            {
                pw = IncrementPassword(pw);
            }

            return pw;
        }

        [Solver(2)]
        public static string Solve2(IEnumerable<ChallengeType> input) =>
            Solve1(new[] { IncrementPassword(Solve1(input)) });
    }
}
