using System;

namespace TypedTree.Generator.Core.Mapping.Exceptions
{
    /// <summary>
    /// Exception for when an enum value is incompatible with scheme rules.
    /// </summary>
    public sealed class InvalidEnumValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumValueException"/> class.
        /// </summary>
        /// <param name="identifier">Identifier of the enum with an invalid value</param>
        /// <param name="value">Value that is invalid</param>
        public InvalidEnumValueException(string identifier, object value)
            : base(message: $"Enum '{identifier}' has invalid value: '{value}', has to be convertible to a int32")
        {
            this.Identifier = identifier;
            this.Value = value;
        }

        /// <summary>
        /// Identifier of the enum with an invalid value.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Value that is invalid.
        /// </summary>
        public object Value { get; }
    }
}
