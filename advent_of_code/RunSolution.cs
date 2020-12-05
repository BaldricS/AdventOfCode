using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace AOC
{
    public class RunSolution
    {
        private readonly int _year;
        private readonly int _day;

        public RunSolution(int year, int day)
        {
            _year = year;
            _day = day;
        }

        public (object, long) Run(MethodInfo mapFunc, MethodInfo solverFunc)
        {
            var input = GetInput(mapFunc);

            var sw = new Stopwatch();
            sw.Start();
            var result = solverFunc.Invoke(null, new[] { input });
            sw.Stop();

            return (result, sw.ElapsedMilliseconds);
        }

        private object GetInput(MethodInfo mapFunc)
        {
            var lines = GetInput();
            if (mapFunc == null)
            {
                return lines;
            }

            return mapFunc.Invoke(null, new[] { lines });
        }

        private string[] GetInput() =>
            File.ReadAllLines(Path.Join("inputs", $"{_year}", $"{_day}"));
    }
}
