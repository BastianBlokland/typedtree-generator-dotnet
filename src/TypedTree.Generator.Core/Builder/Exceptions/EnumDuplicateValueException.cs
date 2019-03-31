using System;

namespace TypedTree.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when an enum with duplicate values is pushed to a scheme.
    /// </summary>
    public sealed class EnumDuplicateValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDuplicateValueException"/> class
        /// </summary>
        /// <param name="identifier">Identifier of the enum with a duplicate value</param>
        public EnumDuplicateValueException(string identifier)
            : base(message: $"Enum '{identifier}' has duplicate values")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Identifier of the enum that has a duplicate value.
        /// </summary>
        public string Identifier { get; }
    }
}
