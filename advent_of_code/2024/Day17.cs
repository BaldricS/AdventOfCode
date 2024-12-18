using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC
{
    public record Input202417()
    {
        public required long RegA { get; set; }
        public required long RegB { get; set; }
        public required long RegC { get; set; }
        public required long[] Program { get; set; }
        public long Instr { get; set; } = 0;
    }

    [AdventOfCode(2024, 17)]
    public static class Day17_2024
    {
        [MapInput]
        public static Input202417 Map(string[] lines)
        {
            return new Input202417(){
                RegA = long.Parse(lines[0].Split(": ")[1]),
                RegB = long.Parse(lines[1].Split(": ")[1]),
                RegC = long.Parse(lines[2].Split(": ")[1]),
                Program = lines[4].Split(": ")[1].Split(',').Select(long.Parse).ToArray()
            };
        }

        [Solver(1)]
        public static string Solve1(Input202417 input)
        {
            return RunProgram(input);
        }

        public static string RunProgram(Input202417 prog)
        {
            var output = new List<long>();
            while (prog.Instr < prog.Program.Length - 1)
            {
                var op = prog.Program[prog.Instr++];
                Console.WriteLine(prog);

                switch (op)
                {
                    case 0:
                        var c0 = prog.Program[prog.Instr++];
                        prog.RegA = (long)(prog.RegA / Math.Pow(2, GetComboValue(prog, c0)));
                        break;
                    case 1:
                        var l1 = prog.Program[prog.Instr++];
                        prog.RegB ^= l1;
                        break;
                    case 2:
                        prog.RegB = GetComboValue(prog, prog.Program[prog.Instr++]) % 8;
                        break;
                    case 3:
                        if (prog.RegA == 0)
                        {
                            // no op?
                            ++prog.Instr;
                        }
                        else
                        {
                            prog.Instr = prog.Program[prog.Instr];
                        }
                        break;
                    case 4:
                        ++prog.Instr;
                        prog.RegB ^= prog.RegC;
                        break;
                    case 5:
                        output.Add(GetComboValue(prog, prog.Program[prog.Instr++]) % 8);
                        break;
                    case 6:
                        var c6 = prog.Program[prog.Instr++];
                        prog.RegB = (long)(prog.RegA / Math.Pow(2, GetComboValue(prog, c6)));
                        break;
                    case 7:
                        var c7 = prog.Program[prog.Instr++];
                        prog.RegC = (long)(prog.RegA / Math.Pow(2, GetComboValue(prog, c7)));
                        break;
                    default:
                        throw new Exception($"Error in program. Current op {op} [{prog}]");
                }
            }

            return string.Join(",", output);
        }

        public static long GetComboValue(Input202417 m, long combo)
        {
            return combo switch
            {
                0 or 1 or 2 or 3 => combo,
                4 => m.RegA,
                5 => m.RegB,
                6 => m.RegC,
                7 => throw new Exception($"Unexecpted combo {combo} [{m}]"),
                _ => throw new Exception($"Unexpected combo {combo} [{m}]"),
            };
        }

        [Solver(2)]
        public static string Solve2(Input202417 input)
        {
            long[] wanted = [2,4,1,1,7,5,0,3,1,4,4,4,5,5,3,0];

            for (int a = 0; a < 7; ++a)
            for (int b = 0; b < 7; ++b)
            for (int c = 0; c < 7; ++c)
            for (int d = 0; d < 7; ++d)
            for (int e = 0; e < 7; ++e)
            for (int f = 0; f < 7; ++f)
            for (int g = 0; g < 7; ++g)
            for (int h = 0; h < 7; ++h)
            for (int i = 0; i < 7; ++i)
            for (int j = 0; j < 7; ++j)
            for (int k = 0; k < 7; ++k)
            for (int l = 0; l < 7; ++l)
            for (int m = 0; m < 7; ++m)
            for (int n = 0; n < 7; ++n)
            for (int o = 0; o < 7; ++o)
            for (int p = 0; p < 7; ++p)
            {
                long maybe = PiecesToNum([p,o,n,m,l,k,j,i,h,g,f,e,d,c,b,a]);

                bool found = true;
                long tmp = maybe;
                foreach (var w in wanted)
                {
                    if (CalculateStep(tmp) != w)
                    {
                        found = false;
                        break;
                    }
                    tmp >>= 3;
                }

                if (found)
                {
                    return $"{maybe}";
                }
            }

            input.RegA = 6175130917;
            return RunProgram(input);
        }

        public static long PiecesToNum(int[] pieces)
        {
            long n = 0;
            foreach (var p in pieces)
            {
                n <<= 3;
                n |= (long)p;
            }

            return n;
        }

/*
2,4 => a % 8 = b
1,1 => b^=1 (Flip lsb, +/- 1)
7,5 => RegC = A / 2^B
0,3 => RegA >>= 3 (Shift out B)
1,4 => RegB ^= 4 (+/- 4, flip 3rd bit)
4,4 => RegB ^= RegC
5,5 => Print RegB % 8
3,0 => Jump if A is done
*/
        public static long CalculateStep(long A)
        {
            long B = A % 8;
            B ^= 1;

            long C = (long)(A / Math.Pow(2, B));
            B ^= 4;
            B ^= C;
            return B % 8;
        }
    }
}