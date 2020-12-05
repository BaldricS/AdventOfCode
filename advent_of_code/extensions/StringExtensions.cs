namespace AOC
{
    public static class StringExtensions
    {
        public static bool IsInRange(this string source, int min, int max)
        {
            if (int.TryParse(source, out int result))
            {
                return result.IsInRange(min, max);
            }

            return false;
        }
    }
}
