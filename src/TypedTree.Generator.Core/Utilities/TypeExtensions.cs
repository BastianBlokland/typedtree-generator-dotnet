using System;
using System.Collections.Generic;
using System.Linq;

namespace TypedTree.Generator.Core.Utilities
{
    /// <summary>
    /// Utility extensions for types.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Try to find an element type for the given type.
        /// </summary>
        /// <remarks>
        /// Works for 'array-like' types:
        /// Array, IReadOnlyList{T}, IReadOnlyCollection{T}, ICollection{T}, IList{T}
        /// </remarks>
        /// <param name="type">Type to find the element-type for</param>
        /// <param name="elementType">Type of element if found, otherwise null</param>
        /// <returns>True if an element-type was found, otherwise false</returns>
        public static bool TryGetElementType(this Type type, out Type elementType)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Array
            if (type.IsArray)
            {
                elementType = type.GetElementType();
                return true;
            }

            // IReadOnlyList<T>
            elementType = GetElementTypeFromInterface(typeof(IReadOnlyList<>));
            if (elementType != null)
                return true;

            // IReadOnlyCollection<T>
            elementType = GetElementTypeFromInterface(typeof(IReadOnlyCollection<>));
            if (elementType != null)
                return true;

            // ICollection<T>
            elementType = GetElementTypeFromInterface(typeof(ICollection<>));
            if (elementType != null)
                return true;

            // IList<T>
            elementType = GetElementTypeFromInterface(typeof(IList<>));
            if (elementType != null)
                return true;

            elementType = null;
            return false;

            Type GetElementTypeFromInterface(Type interfaceType) =>
                type.GetInterface(interfaceType.Name)?.GetGenericArguments()?.FirstOrDefault();
        }
    }
}
