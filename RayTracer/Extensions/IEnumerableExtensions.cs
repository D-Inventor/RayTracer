using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Extensions
{
    public static class IEnumerableExtensions
    {
        public static T Min<T>(this IEnumerable<T> collection, IComparer<T> comparer)
        {
            if (!collection.Any()) { throw new ArgumentException("Collection cannot be empty", nameof(collection)); }

            T result = collection.First();
            foreach (T contestant in collection.Skip(1))
            {
                if (comparer.Compare(result, contestant) > 0)
                {
                    result = contestant;
                }
            }

            return result;
        }
    }
}
