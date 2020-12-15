using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Instructions;
    public record Set(long Location, long Value);
    public record Instructions(string Mask, List<Set> Sets);

    [AdventOfCode(2020, 14)]
    public static class Day14_2020
    {
        [MapInput]
        public static IEnumerable<ChallengeType> Map(string[] lines)
        {
            var data = new List<Instructions>();

            foreach (var l in lines)
            {
                if (l.StartsWith("mask"))
                {
                    data.Add(new Instructions(l.Split(" = ")[1], new List<Set>()));
                }
                else
                {
                    var m = Regex.Match(l, @"mem\[(\d+)\] = (\d+)$");
                    data.Last().Sets.Add(new Set(long.Parse(m.Get(1)), long.Parse(m.Get(2))));
                }
            }

            return data;
        }

        public static (long zeroMask, long oneMask) MaskToNums(string mask)
        {
            long zeroMask = 0;
            long oneMask = 0;

            foreach (var c in mask)
            {
                zeroMask <<= 1;
                oneMask <<= 1;

                if (c == '1')
                {
                    oneMask |= 1;
                    zeroMask |= 1;
                }
                else if (c == 'X')
                {
                    zeroMask |= 1;
                }
            }

            return (zeroMask, oneMask);
        }

        public static string ReplaceAt(string s, int i, char c)
        {
            var chars = s.ToCharArray();
            chars[i] = c;
            return new string(chars);
        }

        public static IEnumerable<string> GetMasks(string mask, int index = 0)
        {
            if (index == mask.Length)
            {
                yield return mask;
            }
            else
            {
                var replacements = mask[index] switch
                {
                    '1' => new[] { '1' },
                    '0' => new[] { 'X' },
                    _ => new[] { '1', '0' }
                };

                var masks = replacements.SelectMany(r => GetMasks(ReplaceAt(mask, index, r), index + 1));
                foreach (var result in masks)
                {
                    yield return result;
                }
            }
        }

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input)
        {
            var programMem = new Dictionary<long, long>();

            foreach (var instruction in input)
            {
                (long zeroMask, long oneMask) = MaskToNums(instruction.Mask);

                foreach (var mem in instruction.Sets)
                {
                    programMem[mem.Location] = (mem.Value & zeroMask) | oneMask;
                }
            }

            return programMem.Sum(kvp => kvp.Value);
        }

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input)
        {
            var programMem = new Dictionary<long, long>();

            foreach (var instruction in input)
            {
                foreach (var mask in GetMasks(instruction.Mask))
                {
                    foreach (var set in instruction.Sets)
                    {
                        (long zeroMask, long oneMask) = MaskToNums(mask);

                        programMem[(set.Location & zeroMask) | oneMask] = set.Value;
                    }
                }
            }

            return programMem.Sum(kvp => kvp.Value);
        }
    }
}
