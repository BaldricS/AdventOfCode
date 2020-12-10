using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2015, 3)]
    public static class Day3_2015
    {
        public static Pair Move(char dir, Pair current) =>
            dir switch
            {
                '^' => current with { Y = current.Y + 1 },
                'v' => current with { Y = current.Y - 1 },
                '>' => current with { X = current.X + 1 },
                _ => current with { X = current.X - 1 }
            };

        [Solver(1)]
        public static int HousesWithPresents(string directions)
        {
            var current = new Pair(0, 0);
            var uniqueHouses = new HashSet<Pair> { current };

            foreach (var dir in directions)
            {
                current = Move(dir, current);
                uniqueHouses.Add(current);
            }

            return uniqueHouses.Count;
        }

        [Solver(2)]
        public static int HousesWithRoboPresents(string directions)
        {
            var santaLoc = new Pair(0, 0);
            var roboSantaLoc = santaLoc;
            var uniqueHouses = new HashSet<Pair> { santaLoc };
            bool moveSanta = true;

            foreach (var dir in directions)
            {
                if (moveSanta)
                {
                    santaLoc = Move(dir, santaLoc);
                    uniqueHouses.Add(santaLoc);
                }
                else
                {
                    roboSantaLoc = Move(dir, roboSantaLoc);
                    uniqueHouses.Add(roboSantaLoc);
                }

                moveSanta = !moveSanta;
            }

            return uniqueHouses.Count;
        }
    }
}
