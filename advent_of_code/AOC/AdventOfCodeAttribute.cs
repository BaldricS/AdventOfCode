using System;

namespace AOC
{
    public class AdventOfCodeAttribute : Attribute
    {
        public int Year { get; }
        public int Day { get; }

        public AdventOfCodeAttribute(int year, int day)
        {
            Year = year;
            Day = day;
        }
    }
}