using System;
using System.Collections.Generic;
using System.Linq;

namespace Xodium.Collections
{
    public static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> collection, T item)
            where T : class
        {
            int index = 0;

            foreach (var element in collection)
            {
                if (element == item)
                    return index;

                index++;
            }

            return -1;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(x => x.First());
        }

        public static IEnumerable<T> Cut<T>(this IList<T> source, int count)
        {
            while (count-- > 0)
            {
                yield return source.FirstOrDefault();
                source.RemoveAt(0);
            }
        }
    }
}
