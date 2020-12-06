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
    }
}
