using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    public static class IEnumerableExtensions
    {
        public static List<T> ToSortedList<T>(this IEnumerable<T> source)
        {
            var result = source.ToList();
            result.Sort();

            return result;
        }

        public static List<R> ToSortedList<T, R>(this IEnumerable<T> source, Func<T, R> map) =>
            source
                .Select(map)
                .ToSortedList();

        public static List<List<T>> Permutations<T>(this IEnumerable<T> source) where T : class
        {
            List<List<T>> PermutationsImpl(List<T> items) =>
                items.Count switch
                {
                    1 => new List<List<T>> { items },
                    _ => items.SelectMany(item =>
                        PermutationsImpl(items.Where(i => i != item).ToList())
                            .Select(p => p.Prepend(item).ToList())
                        ).ToList()
                };

            return PermutationsImpl(source.ToList());
        }

        public static IEnumerable<(T First, T Second)> Pair<T>(this IEnumerable<T> source) =>
            source.Zip(source.Skip(1));

        public static IEnumerable<(T First, T Second)> PairWithWrap<T>(this IEnumerable<T> source) =>
            source.Pair().Append((source.First(), source.Last()));
    }
}
