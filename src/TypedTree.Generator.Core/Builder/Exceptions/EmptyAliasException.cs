using System;

namespace TypedTree.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when an alias without values is pushed to a scheme.
    /// </summary>
    public sealed class EmptyAliasException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyAliasException"/> class
        /// </summary>
        /// <param name="identifier">Identifier of the empty alias</param>
        public EmptyAliasException(string identifier)
            : base(message: $"Alias has no values: '{identifier}'")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Identifier of the alias that was empty.
        /// </summary>
        public string Identifier { get; }
    }
}
