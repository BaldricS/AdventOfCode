using System.Reflection;

namespace AOC
{
    public record Solution(
        MethodInfo MapFunc,
        MethodInfo Solver,
        int Year,
        int Day,
        int Puzzle
    );
}
