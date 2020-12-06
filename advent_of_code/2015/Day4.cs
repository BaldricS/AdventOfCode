using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AOC
{
    [AdventOfCode(2015, 4)]
    public static class Day4_2015
    {
        [Solver(1)]
        public static int FirstMD5(IEnumerable<string> lines)
        {
            var key = lines.First();

            int i = 0;
            while (true)
            {
                var str = $"{key}{i}";
                var bytes = Encoding.ASCII.GetBytes(str);
                var hash = MD5.HashData(bytes);

                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 8)
                {
                    return i;
                }

                ++i;
            }
        }

        [Solver(2)]
        public static int FirstMD52(IEnumerable<string> lines)
        {
            var key = lines.First();

            int i = 0;
            while (true)
            {
                var str = $"{key}{i}";
                var bytes = Encoding.ASCII.GetBytes(str);
                var hash = MD5.HashData(bytes);

                if (hash[0] == 0 && hash[1] == 0 && hash[2] == 0)
                {
                    return i;
                }

                ++i;
            }
        }
    }
}
