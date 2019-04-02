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
            var assembly = Assembly.GetExecutingAssembly();
            var typeCollection = TypeCollection.Create(assembly);
            Assert.NotEqual(0, typeCollection.TypeCount);
            Assert.True(typeCollection.Any());
        }

        [Fact]
        public void OwnTypeCanBeFound()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeCollection = TypeCollection.Create(assembly, includeReferencedAssemblies: false);

            Assert.True(typeCollection.HasType(typeof(TypeCollectionTests)));
            Assert.True(typeCollection.HasType(typeof(TypeCollectionTests).FullName));
            Assert.True(typeCollection.TryGetType(typeof(TypeCollectionTests).FullName, out _));
        }

        [Fact]
        public void ReferencedTypeCanBeFound()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeCollection = TypeCollection.Create(assembly, includeReferencedAssemblies: true);

            Assert.True(typeCollection.HasType(typeof(TypeCollection)));
            Assert.True(typeCollection.HasType(typeof(TypeCollection).FullName));
            Assert.True(typeCollection.TryGetType(typeof(TypeCollection).FullName, out _));
        }
    }
}
