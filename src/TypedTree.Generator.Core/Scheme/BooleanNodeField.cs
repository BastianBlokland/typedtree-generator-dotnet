using System;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of an boolean field on a node.
    /// </summary>
    public sealed class BooleanNodeField : INodeField, IEquatable<BooleanNodeField>
    {
        internal BooleanNodeField(string name, bool isArray)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Invalid identifier: '{name}'", nameof(name));

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
        public static bool operator ==(BooleanNodeField a, BooleanNodeField b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary>Check if two instances are not equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(BooleanNodeField a, BooleanNodeField b) => !(a == b);

        /// <summary>
        /// Check if this is structurally equal to given object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj != null &&
            obj is BooleanNodeField &&
            this.Equals((BooleanNodeField)obj);

        /// <summary>
        /// Check if this is structurally equal to given other boolean-field.
        /// </summary>
        /// <param name="other">Boolean-field to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(BooleanNodeField other) =>
            other != null &&
            other.Name == this.Name &&
            other.IsArray == this.IsArray;

        /// <summary>
        /// Get a hashcode representing this boolean-field.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(
            this.Name,
            this.IsArray);

        /// <summary>
        /// Get a string representation for this field.
        /// </summary>
        public override string ToString() =>
            $"{this.Name}:bool{(this.IsArray ? "[]" : string.Empty)}";
    }
}
