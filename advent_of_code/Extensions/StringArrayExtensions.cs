using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    public static class StringArrayExtensions
    {
        public static IEnumerable<List<string>> GatherByNewline(this string[] source) =>
            source.GatherByNewline(x => x);

        public static IEnumerable<List<T>> GatherByNewline<T>(this string[] source, Func<string, T> mapFunc) =>
            source.Aggregate(
                new List<List<T>>() { new List<T>() },
                (acc, next) =>
                {
                    if (next.Length == 0)
                    {
                        acc.Add(new List<T>());
                    }
                    else
                    {
                        acc.Last().Add(mapFunc(next));
                    }

                    return acc;
                });


    }
}
