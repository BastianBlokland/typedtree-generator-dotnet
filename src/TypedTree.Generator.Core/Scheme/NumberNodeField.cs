using System;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of a number field on a node.
    /// </summary>
    public sealed class NumberNodeField : INodeField, IEquatable<NumberNodeField>
    {
        internal NumberNodeField(string name, bool isArray)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Invalid name: '{name}'", nameof(name));

            this.Name = name;
            this.IsArray = isArray;
        }

        /// <summary>
        /// Identifier for this field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Is this field an array.
        /// </summary>
        public bool IsArray { get; }

        /// <summary>Check if two instances are equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool operator ==(NumberNodeField a, NumberNodeField b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary>Check if two instances are not equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(NumberNodeField a, NumberNodeField b) => !(a == b);

        /// <summary>
        /// Check if this is structurally equal to given object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj != null &&
            obj is NumberNodeField &&
            this.Equals((NumberNodeField)obj);

        /// <summary>
        /// Check if this is structurally equal to given other number-field.
        /// </summary>
        /// <param name="other">Number-field to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(NumberNodeField other) =>
            other != null &&
            other.Name == this.Name &&
            other.IsArray == this.IsArray;

        /// <summary>
        /// Get a hashcode representing this number-field.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(
            this.Name,
            this.IsArray);
    }
}
