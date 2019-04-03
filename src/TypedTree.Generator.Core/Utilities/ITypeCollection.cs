using System;
using System.Collections.Generic;

namespace TypedTree.Generator.Core.Utilities
{
    /// <summary>
    /// Abstraction around a set of types.
    /// </summary>
    public interface ITypeCollection : IEnumerable<Type>
    {
        /// <summary>
        /// How many types are in this set.
        /// </summary>
        int TypeCount { get; }

        /// <summary>
        /// Check if specified type is included in this set.
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if found, otherwise false</returns>
        bool HasType(Type type);

        /// <summary>
        /// Check if a type with specified name is included in this set.
        /// </summary>
        /// <param name="typeName">Name of the type to check</param>
        /// <returns>True if found, otherwise false</returns>
        bool HasType(string typeName);

        /// <summary>
        /// Try to get type by name.
        /// </summary>
        /// <param name="typeName">Name of the type to get</param>
        /// <param name="type">Found type</param>
        /// <returns>True if found, otherwise false</returns>
        bool TryGetType(string typeName, out Type type);
    }
}
