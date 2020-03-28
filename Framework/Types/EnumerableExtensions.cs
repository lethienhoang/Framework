using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Types
{
    public static class EnumerableExtensions
    {
        public static IList<List<T>> SplitInBatches<T>(this List<T> items, int batchSize)
        {
            var list = new List<List<T>>();

            if (batchSize == 0)
            {
                return list;
            }

            for (int i = 0; i < items.Count; i += batchSize)
            {
                list.Add(items.GetRange(i, Math.Min(batchSize, items.Count - i)));
            }

            return list;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> key)
            where TSource : class
        {
            var keys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (keys.Add(key(element)))
                {
                    yield return element;
                }
            }
        }

        public static void Each<TSource>(this List<TSource> source, Action<TSource> action)
            where TSource : class
        {
            foreach (var element in source)
            {
                action(element);
            }
        }

        public static void Each<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
            where TSource : class
        {
            foreach (var element in source)
            {
                action(element);
            }
        }
    }
}
