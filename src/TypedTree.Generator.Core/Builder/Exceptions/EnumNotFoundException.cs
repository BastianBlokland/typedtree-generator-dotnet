using System;

namespace TypedTree.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when an enum cannot be found.
    /// </summary>
    public sealed class EnumNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumNotFoundException"/> class.
        /// </summary>
        /// <param name="identifier">Identifier of missing enum</param>
        public EnumNotFoundException(string identifier)
            : base(message: $"Enum '{identifier}' not found")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Identifier of the enum that was not found.
        /// </summary>
        public string Identifier { get; }
    }
}
