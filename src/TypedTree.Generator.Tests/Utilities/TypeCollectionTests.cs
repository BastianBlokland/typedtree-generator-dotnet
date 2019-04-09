using System.Reflection;
using System.Linq;
using Xunit;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Tests.Utilities
{
    public sealed class TypeCollectionTests
    {
        [Fact]
        public void AssemblyCanBeLoaded()
        {
            var typeCollection = TypeCollection.Create(typeof(TypeCollectionTests).Assembly);
            Assert.NotEqual(0, typeCollection.TypeCount);
            Assert.True(typeCollection.Any());
        }

        [Fact]
        public void TypeCanBeFound()
        {
            var typeCollection = TypeCollection.Create(typeof(TypeCollectionTests).Assembly);

            Assert.True(typeCollection.HasType(typeof(TypeCollectionTests)));
            Assert.True(typeCollection.HasType(typeof(TypeCollectionTests).FullName));
            Assert.True(typeCollection.TryGetType(typeof(TypeCollectionTests).FullName, out _));
        }
    }
}
