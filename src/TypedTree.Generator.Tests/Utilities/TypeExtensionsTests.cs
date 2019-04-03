#pragma warning disable CA1812 // Avoid uninstantiated internal classes

using System.Text.RegularExpressions;
using System.Reflection;
using System.Linq;
using Xunit;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Tests.Utilities
{
    public sealed class TypeExtensionsTests
    {
        private interface ITestInterface
        {
        }

        [Fact]
        public void GetImplementations_OnlyConstructableImplementationsAreFound()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeCollection = TypeCollection.Create(assembly);

            var implementations = typeCollection.GetImplementations(typeof(ITestInterface));
            Assert.Equal(2, implementations.Count());
            Assert.Contains(typeof(TestImplementationB), implementations);
            Assert.Contains(typeof(TestImplementationC), implementations);
        }

        [Fact]
        public void GetImplementations_IgnoredTypesAreNotFound()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeCollection = TypeCollection.Create(assembly);

            var ignoreRegex = new Regex(".*B$");
            var implementations = typeCollection.GetImplementations(typeof(ITestInterface), ignoreRegex);
            Assert.Equal(new[] { typeof(TestImplementationC) }, implementations);
        }

        [Fact]
        public void GetImplementations_SingleTypeImplementionIsFound()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeCollection = TypeCollection.Create(assembly);

            var implementations = typeCollection.GetImplementations(typeof(TestStruct));
            Assert.Equal(new[] { typeof(TestStruct) }, implementations);
        }

        [Fact]
        public void GetImplementations_AbstractClassesAreNotReturned()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeCollection = TypeCollection.Create(assembly);

            var implementations = typeCollection.GetImplementations(typeof(TestSingleAbstractClass));
            Assert.Empty(implementations);
        }

        private struct TestStruct
        {
        }

        private abstract class TestAbstractClassA : ITestInterface
        {
        }

        private class TestImplementationB : TestAbstractClassA
        {
        }

        private class TestImplementationC : ITestInterface
        {
        }

        private abstract class TestSingleAbstractClass
        {
        }
    }
}
