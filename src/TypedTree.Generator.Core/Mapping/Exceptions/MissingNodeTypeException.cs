using System;

namespace TypedTree.Generator.Core.Mapping.Exceptions
{
    /// <summary>
    /// Exception for when the type of a node in an alias cannot be found.
    /// </summary>
    public sealed class MissingNodeTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingNodeTypeException"/> class.
        /// </summary>
        /// <param name="identifier">Identifier of the missing node-type</param>
        public MissingNodeTypeException(string identifier)
            : base(message: $"Type for node '{identifier}' cannot be found")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Identifier of node-type that cannot be found.
        /// </summary>
        public string Identifier { get; }
    }
}
