using System.Text.RegularExpressions;

namespace AOC
{
    public static class MatchExtensions
    {
        public static string Get(this Match source, int index) =>
            source.Groups[index].Value;

        public static Pair GetPair(this Match source, int first) =>
            new Pair(source.Get(first).AsInt(), source.Get(first + 1).AsInt());
    }
}
