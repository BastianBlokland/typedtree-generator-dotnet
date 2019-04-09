using System;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of an alias field on a node.
    /// </summary>
    public sealed class AliasNodeField : INodeField, IEquatable<AliasNodeField>
    {
        internal AliasNodeField(string name, AliasDefinition alias, bool isArray)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Invalid name: '{name}'", nameof(name));
            if (alias == null)
                throw new ArgumentNullException(nameof(alias));

            this.Name = name;
            this.Alias = alias;
            this.IsArray = isArray;
        }

        /// <summary>
        /// Identifier for this field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Get the Alias to a node that this field can contain.
        /// </summary>
        public AliasDefinition Alias { get; }

        /// <summary>
        /// Is this field an array.
        /// </summary>
        public bool IsArray { get; }

        /// <summary>Check if two instances are equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool operator ==(AliasNodeField a, AliasNodeField b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary>Check if two instances are not equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(AliasNodeField a, AliasNodeField b) => !(a == b);

        /// <summary>
        /// Check if this is structurally equal to given object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj != null &&
            obj is AliasNodeField &&
            this.Equals((AliasNodeField)obj);

        /// <summary>
        /// Check if this is structurally equal to given other alias-field.
        /// </summary>
        /// <param name="other">Alias-field to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(AliasNodeField other) =>
            other != null &&
            other.Name == this.Name &&
            other.Alias == this.Alias &&
            other.IsArray == this.IsArray;

        /// <summary>
        /// Get a hashcode representing this alias-field.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(
            this.Name,
            this.Alias,
            this.IsArray);

        /// <summary>
        /// Get a string representation for this field.
        /// </summary>
        public override string ToString() =>
            $"{this.Name}:{this.Alias.Identifier}{(this.IsArray ? "[]" : string.Empty)}";
    }
}
