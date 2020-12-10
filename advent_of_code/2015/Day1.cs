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
        public static long GetFloor(string elevator) => CountUp(elevator) - CountDown(elevator);

        [Solver(2)]
        public static long GetFirstBasement(string elevator)
        {
            long currentFloor = 0;

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