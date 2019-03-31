using System;

namespace TypedTree.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when an enum without entries is pushed to a scheme.
    /// </summary>
    public sealed class EmptyEnumException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyEnumException"/> class
        /// </summary>
        /// <param name="identifier">Identifier of the empty enum</param>
        public EmptyEnumException(string identifier)
            : base(message: $"Enum has no entries: '{identifier}'")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Identifier of the enum that has no entries.
        /// </summary>
        public string Identifier { get; }
    }
}
