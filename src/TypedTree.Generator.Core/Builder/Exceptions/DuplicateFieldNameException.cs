using System;

namespace TypedTree.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when two fields with identical names are added to the same node.
    /// </summary>
    public sealed class DuplicateFieldNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateFieldNameException"/> class
        /// </summary>
        /// <param name="nodeType">Type of the node that the duplicated happend on</param>
        /// <param name="fieldName">Fieldname that is duplicated</param>
        public DuplicateFieldNameException(string nodeType, string fieldName)
            : base(message: $"Duplicate field-name '{fieldName}' on node: '{nodeType}'")
        {
            this.NodeType = nodeType;
            this.FieldName = fieldName;
        }

        /// <summary>
        /// Type of the node that this occurred on.
        /// </summary>
        public string NodeType { get; }

        /// <summary>
        /// Fieldname that is duplicated.
        /// </summary>
        public string FieldName { get; }
    }
}
