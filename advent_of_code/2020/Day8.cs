using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC
{
    using ChallengeType = Instruction;

    public record Instruction(string Name, int Value);

    [AdventOfCode(2020, 8)]
    public static class Day8_2020
    {
        [MapInput]
        public static IEnumerable<ChallengeType> Map(string[] lines) =>
            lines
                .Select(l => Regex.Match(l, @"^(.*?) ([-+]\d+)$"))
                .Select(m => new ChallengeType(m.Get(1), m.Get(2).AsInt()));

        public static (long, bool) RunProgram(IEnumerable<ChallengeType> input)
        {
            long acc = 0;
            int iPtr = 0;
            var alreadyRun = new HashSet<int>();
            var instructions = input.ToArray();

            int ExecuteAcc(int value)
            {
                acc += value;
                return 1;
            }

            while (iPtr < instructions.Length)
            {
                if (alreadyRun.Contains(iPtr))
                {
                    return (acc, true);
                }

                alreadyRun.Add(iPtr);
                var instr = instructions[iPtr];

                iPtr += instr.Name switch
                {
                    "acc" => ExecuteAcc(instr.Value),
                    "jmp" => instr.Value,
                    _ => 1
                };
            }

            return (acc, false);
        }

        [Solver(1)]
        public static long Solve1(IEnumerable<ChallengeType> input) => RunProgram(input).Item1;

        [Solver(2)]
        public static long Solve2(IEnumerable<ChallengeType> input)
        {
            var instructions = input.ToArray();

            for (int i = 0; i < instructions.Length; ++i)
            {
                if (instructions[i].Name == "nop")
                {
                    instructions[i] = instructions[i] with { Name = "jmp" };

                    var result = RunProgram(instructions);
                    if (!result.Item2)
                    {
                        return result.Item1;
                    }

                    instructions[i] = instructions[i] with { Name = "nop" };
                }
                else if (instructions[i].Name == "jmp")
                {
                    instructions[i] = instructions[i] with { Name = "nop" };

                    var result = RunProgram(instructions);
                    if (!result.Item2)
                    {
                        return result.Item1;
                    }

                    instructions[i] = instructions[i] with { Name = "jmp" };
                }
            }

            return 1;
        }
    }
}
