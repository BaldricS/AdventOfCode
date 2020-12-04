using System;

namespace AOC
{
    public class SolverAttribute : Attribute
    {
        public int Puzzle { get; }

        public SolverAttribute(int puzzle)
        {
            Puzzle = puzzle;
        }
    }
}