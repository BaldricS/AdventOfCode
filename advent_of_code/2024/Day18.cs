using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC
{
    public record Input202418()
    {
        public string[] Lines { get; set; }
    }

    [AdventOfCode(2024, 18)]
    public static class Day18_2024
    {
        [MapInput]
        public static Input202418 Map(string[] lines)
        {
            return new Input202418(){
                Lines = lines
            };
        }

        [Solver(1)]
        public static int Solve1(Input202418 input)
        {
            return 1;
        }

        [Solver(2)]
        public static int Solve2(Input202418 input)
        {
            return 1;
        }
    }
}