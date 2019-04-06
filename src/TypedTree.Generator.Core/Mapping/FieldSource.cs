namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Source where the gather fields from.
    /// </summary>
    public enum FieldSource
    {
        /// <summary>
        /// Gather fields from public properties on a type.
        /// </summary>
        PublicProperties,

        /// <summary>
        /// Gather fields from properties (including non-public) on a type.
        /// </summary>
        Properties,

        /// <summary>
        /// Gather fields from the parameters of the public constructor with the most parameters.
        /// </summary>
        PublicConstructorParameters,

        /// <summary>
        /// Gather fields from the parameters of the constructor (including non-public) with
        /// the most parameters.
        /// </summary>
        ConstructorParameters
    }
}
