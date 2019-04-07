using System;

namespace TypedTree.Generator.Core.Mapping.Exceptions
{
    /// <summary>
    /// Exception for when the same alias identifier is used twice but with different implementations.
    /// </summary>
    public sealed class DuplicateAliasException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateAliasException"/> class.
        /// </summary>
        /// <param name="identifier">Identifier of the duplicated alias</param>
        public DuplicateAliasException(string identifier)
            : base(message: $"Alias '{identifier}' was used twice but with different implementations")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Alias identifier that was used twice.
        /// </summary>
        public string Identifier { get; }
    }
}
