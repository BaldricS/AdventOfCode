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
        public static long Solve2(Input202417 input)
        {
            int[] wanted = [2,4,1,1,7,5,0,3,1,4,4,4,5,5,3,0];
            int?[] bits = Enumerable.Range(0, 64).Select(_ => (int?)null).ToArray();

            HashSet<long> validNumbers = [];

            FindNumbers(0, wanted, bits, validNumbers);

            Console.WriteLine(validNumbers.Count);
            foreach (var n in validNumbers)
            {
                Console.WriteLine(n);
            }

            Console.WriteLine(validNumbers.Max());

            return validNumbers.Min();
        }

        public static void FindNumbers(int wIdx, int[] wanted, int?[] bits, HashSet<long> validNumbers)
        {
            // Done writing
            if (wIdx == wanted.Length)
            {
                validNumbers.Add(bits.Aggregate(0L, (n, b) => (n << 1) | (b ?? 0)));
                return;
            }

            int[] b4Cases = [0b101, 0b100, 0b111, 0b110, 0b001, 0b000, 0b011, 0b010];

            int w = wanted[wIdx];
            int bOffset = bits.Length - 3 * (wIdx + 1);

            foreach (var b4 in b4Cases)
            {
                int cLast3 = w ^ b4;
                int?[] copiedBits = [.. bits];

                if (WriteBits(cLast3, b4, copiedBits, bOffset))
                {
                    FindNumbers(wIdx + 1, wanted, copiedBits, validNumbers);
                }
            }
        }

        public static bool WriteBits(int c, int b4Case, int?[] bits, int startOfb)
        {
            if (b4Case == 0b101)
            {
                return TryWriteLoop(bits, 0, startOfb) && TryWriteLoop(bits, c, startOfb - 1);
            }

            if (b4Case == 0b100)
            {
                return TryWriteLoop(bits, 0b001, startOfb) && TryWriteLoop(bits, c, startOfb);
            }

            if (b4Case == 0b111)
            {
                return TryWriteLoop(bits, 0b010, startOfb) && TryWriteLoop(bits, c, startOfb - 3);
            }

            if (b4Case == 0b110)
            {
                return TryWriteLoop(bits, 0b011, startOfb) && TryWriteLoop(bits, c, startOfb - 2);
            }

            if (b4Case == 0b001)
            {
                return TryWriteLoop(bits, 0b100, startOfb) && TryWriteLoop(bits, c, startOfb - 5);
            }

            if (b4Case == 0b000)
            {
                return TryWriteLoop(bits, 0b101, startOfb) && TryWriteLoop(bits, c, startOfb - 4);
            }

            if (b4Case == 0b011)
            {
                return TryWriteLoop(bits, 0b110, startOfb) && TryWriteLoop(bits, c, startOfb - 7);
            }

            if (b4Case == 0b010)
            {
                return TryWriteLoop(bits, 0b111, startOfb) && TryWriteLoop(bits, c, startOfb - 6);
            }

            throw new Exception($"case: {b4Case}");
        }

        public static bool TryWriteLoop(int?[] bits, int value, int idx)
        {
            for (int i = 0; i < 3; ++i)
            {
                if (!TryWrite(bits, (value >> (2 - i)) & 1, idx + i))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool TryWrite(int?[] bits, int value, int idx)
        {
            if (idx < 0 || idx >= bits.Length)
            {
                return false;
            }

            if (!bits[idx].HasValue)
            {
                bits[idx] = value;
            }

            return bits[idx].Value == value;
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

            long C = A >> (int)B;
            B ^= 4;
            B ^= C;
            return B % 8;
        }
    }
}