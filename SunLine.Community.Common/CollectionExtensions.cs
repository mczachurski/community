using System;
using System.Collections.Generic;

namespace SunLine.Community.Common
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (T item in items)
            {
                list.Add(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (T item in source)
            {
                action(item);
            }
        }
    }
}

