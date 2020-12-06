using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    [AdventOfCode(2015, 3)]
    public static class Day3_2015
    {
        public static Pair Move(char dir, Pair current)
        {
            switch (dir)
            {
                case '^':
                    return current with { Y = current.Y + 1 };
                case 'v':
                    return current with { Y = current.Y - 1 };
                case '>':
                    return current with { X = current.X + 1 };
                default:
                    return current with { X = current.X - 1 };
            }
        }

        [Solver(1)]
        public static int HousesWithPresents(IEnumerable<string> lines)
        {
            var directions = lines.First();

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
        public static int HousesWithRoboPresents(IEnumerable<string> lines)
        {
            var directions = lines.First();

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
