using System.Reflection;
using Xunit;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Tests.Utilities
{
    public sealed class TypeSetTests
    {
        [Fact]
        public void AssemblyCanBeLoaded()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeSet = TypeSet.Create(assembly);
            Assert.NotEqual(0, typeSet.TypeCount);
        }

        [Fact]
        public void OwnTypeCanBeFound()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeSet = TypeSet.Create(assembly, includeReferencedAssemblies: false);

            Assert.True(typeSet.HasType(typeof(TypeSetTests)));
            Assert.True(typeSet.HasType(typeof(TypeSetTests).FullName));
            Assert.True(typeSet.TryGetType(typeof(TypeSetTests).FullName, out _));
        }

        [Fact]
        public void ReferencedTypeCanBeFound()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeSet = TypeSet.Create(assembly, includeReferencedAssemblies: true);

            Assert.True(typeSet.HasType(typeof(TypeSet)));
            Assert.True(typeSet.HasType(typeof(TypeSet).FullName));
            Assert.True(typeSet.TryGetType(typeof(TypeSet).FullName, out _));
        }
    }
}
