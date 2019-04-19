using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace TypedTree.Generator.Core.Utilities
{
    /// <summary>
    /// Immutable abstraction around a set of types.
    /// </summary>
    /// <remarks>
    /// Types are stored by full-name so if multiple assemblies include types with the same full-name
    /// then only 1 can be found.
    /// </remarks>
    public sealed class TypeCollection : ITypeCollection
    {
        private readonly ImmutableArray<Type> types;
        private readonly ImmutableDictionary<string, Type> typeLookup;

        internal TypeCollection(IEnumerable<Type> types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            this.types = types.ToImmutableArray();
            this.typeLookup = types.ToImmutableDictionary(t => t.FullName);
        }

        /// <summary>
        /// How many types are in this set.
        /// </summary>
        public int TypeCount => this.types.Length;

        /// <summary>
        /// Create a type-collection from a collection of assemblies.
        /// </summary>
        /// <param name="assemblies">Assemblies to get types from</param>
        /// <returns>Newly created immutable typecollection</returns>
        public static TypeCollection Create(params Assembly[] assemblies) =>
            Create(assemblies as IEnumerable<Assembly>, logger: null);

        /// <summary>
        /// Create a type-collection from a collection of assemblies.
        /// </summary>
        /// <param name="assemblies">Assemblies to get types from</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>Newly created immutable typecollection</returns>
        public static TypeCollection Create(IEnumerable<Assembly> assemblies, ILogger logger = null)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            return new TypeCollection(
                types: assemblies.SelectMany(GetTypes).Distinct(TypeNameComparer.Instance));

            IEnumerable<Type> GetTypes(Assembly assembly)
            {
                if (assembly == null)
                    return Array.Empty<Type>();

                Type[] types = null;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (Exception e)
                {
                    logger?.LogWarning($"Failed to load types '{assembly.FullName}': '{e.Message}'");
                }

                return types?.Where(IsValidType) ?? Array.Empty<Type>();
            }

            bool IsValidType(Type type)
            {
                try
                {
                    return
                        type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() == 0 &&
                        type.Namespace != null;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Get an enumerator to iterate over all the types in this set.
        /// </summary>
        public IEnumerator<Type> GetEnumerator() => ((IEnumerable<Type>)this.types).GetEnumerator();

        /// <summary>
        /// Check if specified type is included in this set.
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if found, otherwise false</returns>
        public bool HasType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return this.typeLookup.ContainsKey(type.FullName);
        }

        /// <summary>
        /// Check if a type with specified name is included in this set.
        /// </summary>
        /// <param name="typeName">Name of the type to check</param>
        /// <returns>True if found, otherwise false</returns>
        public bool HasType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentException($"Invalid type-name: '{typeName}'", nameof(typeName));

            return this.typeLookup.ContainsKey(typeName);
        }

        /// <summary>
        /// Try to get type by name.
        /// </summary>
        /// <param name="typeName">Name of the type to get</param>
        /// <param name="type">Found type</param>
        /// <returns>True if found, otherwise false</returns>
        public bool TryGetType(string typeName, out Type type)
        {
            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentException($"Invalid type-name: '{typeName}'", nameof(typeName));

            return this.typeLookup.TryGetValue(typeName, out type);
        }

        /// <summary>
        /// Get an enumerator to iterate over all the types in this set.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private sealed class TypeNameComparer : IEqualityComparer<Type>
        {
            public static TypeNameComparer Instance { get; } = new TypeNameComparer();

            public bool Equals(Type a, Type b) => a.FullName == b.FullName;

            public int GetHashCode(Type type) => type.FullName.GetHashCode();
        }
    }
}
