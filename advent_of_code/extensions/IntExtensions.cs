namespace AOC
{
    public static class IntExtensions
    {
        public static bool IsInRange(this int source, int low, int high) => source >= low && source <= high;
    }
}
