#pragma warning disable CA1812 // Avoid uninstantiated internal classes

using System;
using System.Collections.Generic;
using Xunit;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Tests.Utilities
{
    public sealed class TypeExtensionsTests
    {
        public static IEnumerable<object[]> GetTypeElementPairsData()
        {
            foreach (var tuple in GetTypeElementPairs())
                yield return new object[] { tuple.inputType, tuple.elementType };
        }

        [Theory]
        [MemberData(nameof(GetTypeElementPairsData))]
        public void ElementTypesAreFound(Type inputType, Type expectedElementType)
        {
            inputType.TryGetElementType(out var elementType);

            Assert.Equal(expectedElementType, elementType);
        }

        private static IEnumerable<(Type inputType, Type elementType)> GetTypeElementPairs()
        {
            yield return (typeof(int[]), typeof(int));
            yield return (typeof(Stack<float>), typeof(float));
            yield return (typeof(List<string>), typeof(string));
            yield return (typeof(Queue<double>), typeof(double));
            yield return (typeof(HashSet<byte>), typeof(byte));
            yield return (typeof(Dictionary<byte, string>), typeof(KeyValuePair<byte, string>));
            yield return (typeof(List<TypeExtensionsTests>), typeof(TypeExtensionsTests));

            yield return (typeof(int), null);
            yield return (typeof(string), null);
            yield return (typeof(TypeExtensionsTests), null);
        }
    }
}
