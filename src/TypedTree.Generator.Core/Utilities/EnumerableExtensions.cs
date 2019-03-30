using System.Collections.Generic;
using System.Linq;

namespace TypedTree.Generator.Core.Utilities
{
    /// <summary>
    /// Utility extensions for IEnumerable and IEnumerable{T}
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Check if all entries in a enumerable are unique. (Using default equality)
        /// </summary>
        /// <param name="enumerable">Enumerable to check</param>
        /// <typeparam name="T">Type of the elements in the enumerable</typeparam>
        /// <returns>True if all elements are unique, otherwise false</returns>
        public static bool IsUnique<T>(this IEnumerable<T> enumerable) =>
            IsUnique(enumerable, EqualityComparer<T>.Default);

        /// <summary>
        /// Check if all entries in a enumerable are unique.
        /// </summary>
        /// <param name="enumerable">Enumerable to check</param>
        /// <param name="comparer">Comparer to verify uniqueness</param>
        /// <typeparam name="T">Type of the elements in the enumerable</typeparam>
        /// <returns>True if all elements are unique, otherwise false</returns>
        public static bool IsUnique<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer) =>
            enumerable.All(new HashSet<T>(comparer).Add);
    }
}
