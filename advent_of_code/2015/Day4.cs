using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AOC
{
    [AdventOfCode(2015, 4)]
    public static class Day4_2015
    {
        public static int FindHash(string key, Func<byte[], bool> IsWinner)
        {
            int i = 0;
            while (true)
            {
                var str = $"{key}{i}";
                var bytes = Encoding.ASCII.GetBytes(str);
                var hash = MD5.HashData(bytes);

                if (IsWinner(hash))
                {
                    return i;
                }

                ++i;
            }
        }

        [Solver(1)]
        public static int FirstMD5(IEnumerable<string> lines) =>
            FindHash(lines.First(), bytes => bytes[0] == 0 && bytes[1] == 0 && bytes[2] < 8);

        [Solver(2)]
        public static int FirstMD52(IEnumerable<string> lines) =>
            FindHash(lines.First(), bytes => bytes[0] == 0 && bytes[1] == 0 && bytes[2] == 0);
    }
}
