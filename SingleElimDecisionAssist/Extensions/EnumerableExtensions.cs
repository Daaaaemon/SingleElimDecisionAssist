using System;
using System.Collections.Generic;
using System.Linq;

namespace SingleElimDecisionAssist.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (rng == null) throw new ArgumentNullException(nameof(rng));

            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }

        public static IEnumerable<TResult> SelectTwo<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using var iterator = source.GetEnumerator();
            var item2 = default(TSource);
            var i = 0;
            while (iterator.MoveNext())
            {
                var item1 = item2;
                item2 = iterator.Current;
                i++;

                if (i >= 2)
                {
                    yield return selector(item1, item2);
                }
            }
        }
    }
}
