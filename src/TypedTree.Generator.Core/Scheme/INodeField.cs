namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Field on a node.
    /// </summary>
    public interface INodeField
    {
        /// <summary>
        /// Identifier of this field.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Is this field an array.
        /// </summary>
        bool IsArray { get; }
    }
}
