using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AOC
{
    [AdventOfCode(2024, 4)]
    public static class Day4_2024
    {
        [MapInput]
        public static string[] Map(string[] lines) => lines;

        [Solver(1)]
        public static int Solve1(string[] mem)
        {
            string forward = "XMAS";
            string rev = "SAMX";

            return mem.Select(l => CountHorizontal(l, forward) + CountHorizontal(l, rev)).Sum() +
                CountVertical(mem, forward) + CountVertical(mem, rev) +
                CountNegativeDiagonal(mem, forward) + CountNegativeDiagonal(mem, rev) +
                CountPositiveDiagonal(mem, forward) + CountPositiveDiagonal(mem, rev)
            ;
        }

        private static int CountHorizontal(string line, string needle)
        {
            return new Regex(needle).Matches(line).Count;
        }

        private static int CountVertical(string[] lines, string needle)
        {
            int count = 0;

            for (int r = 0; r <= lines.Length - needle.Length; ++r)
            {
                for (int c = 0; c < lines[r].Length; ++c)
                {
                    bool found = true;

                    for (int n = 0; n < needle.Length; ++n)
                    {
                        if (lines[r + n][c] != needle[n])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        ++count;
                    }
                }
            }

            return count;
        }

        private static int CountNegativeDiagonal(string[] lines, string needle)
        {
            int count = 0;
            for (int r = 0; r <= lines.Length - needle.Length; ++r)
            {
                for (int c = 0; c <= lines[r].Length - needle.Length; ++c)
                {
                    bool found = true;
                    for (int n = 0; n < needle.Length; ++n)
                    {
                        if(lines[r + n][c + n] != needle[n])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        ++count;
                    }
                }
            }

            return count;
        }

        private static int CountPositiveDiagonal(string[] lines, string needle)
        {
            Console.WriteLine($"Positive for {needle}");
            int count = 0;
            for (int r = 0; r <= lines.Length - needle.Length; ++r)
            {
                for (int c = needle.Length - 1; c < lines[r].Length; ++c)
                {
                    bool found = true;
                    for (int n = 0; n < needle.Length; ++n)
                    {
                        if(lines[r + n][c - n] != needle[n])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        ++count;
                    }
                }
            }

            return count;
        }

        [Solver(2)]
        public static int Solve2(string[] lines)
        {
            return FindXMas(lines);
        }

        private static int FindXMas(string[] lines)
        {
            int count = 0;
            for (int r = 1; r < lines.Length - 1; ++r)
            {
                for (int c = 1; c < lines[r].Length - 1; ++c)
                {
                    if (lines[r][c] == 'A')
                    {
                        bool hasNegative = lines[r - 1][c - 1] == 'M' && lines[r + 1][c + 1] == 'S' ||
                            lines[r - 1][c - 1] == 'S' && lines[r + 1][c + 1] == 'M';
                        bool hasPositive = lines[r - 1][c + 1] == 'M' && lines[r + 1][c - 1] == 'S' ||
                            lines[r - 1][c + 1] == 'S' && lines[r + 1][c - 1] == 'M';
                    
                        if (hasNegative && hasPositive)
                        {
                            ++count;
                        }
                    }
                }
            }

            return count;
        } 
    }
}