using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2015, 1)]
    public static class Day1_2015
    {
        public static long CountUp(string line) => line.Count(c => c == '(');
        
        public static long CountDown(string line) => line.Count(c => c == ')');

        [Solver(1)]
        public static long GetFloor(IEnumerable<string> lines) =>
            lines.Select(line => CountUp(line) - CountDown(line)).Sum();

        [Solver(2)]
        public static long GetFirstBasement(IEnumerable<string> lines)
        {
            long currentFloor = 0;
            string elevator = lines.First();

            for (int i = 0; i < elevator.Length; ++i)
            {
                currentFloor += elevator[i] == '(' ? 1 : -1;
               
                if (currentFloor == -1)
                {
                    return i + 1;
                }
            }

            return 0;
        }
    }
}