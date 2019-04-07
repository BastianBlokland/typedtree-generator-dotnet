using System;

namespace TypedTree.Generator.Core.Mapping.Exceptions
{
    /// <summary>
    /// Exception for when the type of a node / alias cannot be found.
    /// </summary>
    public sealed class MissingTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingTypeException"/> class.
        /// </summary>
        /// <param name="identifier">Identifier of the missing type</param>
        public MissingTypeException(string identifier)
            : base(message: $"Type for node / alias '{identifier}' cannot be found")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Identifier of type that cannot be found.
        /// </summary>
        public string Identifier { get; }
    }
}
