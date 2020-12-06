namespace AOC
{
    public static class StringExtensions
    {
        public static bool IsInRange(this string source, int min, int max) =>
            int.TryParse(source, out int result)
            ? result.IsInRange(min, max)
            : false;
    }
}
