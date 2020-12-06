using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    [AdventOfCode(2015, 3)]
    public static class Day3_2015
    {
        public record Location(int Row, int Col);

        public static Location Move(char dir, Location current)
        {
            switch (dir)
            {
                case '^':
                    return current with { Col = current.Col + 1 };
                case 'v':
                    return current with { Col = current.Col - 1 };
                case '>':
                    return current with { Row = current.Row + 1 };
                default:
                    return current with { Row = current.Row - 1 };
            }
        }

        [Solver(1)]
        public static int HousesWithPresents(IEnumerable<string> lines)
        {
            var directions = lines.First();

            var current = new Location(0, 0);
            var uniqueHouses = new HashSet<Location> { current };

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

            var santaLoc = new Location(0, 0);
            var roboSantaLoc = santaLoc;
            var uniqueHouses = new HashSet<Location> { santaLoc };
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
