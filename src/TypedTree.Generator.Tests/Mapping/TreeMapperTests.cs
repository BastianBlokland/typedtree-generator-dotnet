#pragma warning disable CA1812 // Avoid uninstantiated internal classes

using System.Collections.Generic;
using System.Reflection;
using Xunit;

using TypedTree.Generator.Core.Mapping;
using TypedTree.Generator.Core.Builder;

namespace TypedTree.Generator.Tests.Mapping
{
    public sealed class TreeMapperTests
    {
        private interface IRootInterface
        {
        }

        private interface IInnerInterface
        {
        }

        [Fact]
        public void TreeIsMappedAccordingToSchemeRules()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var context = Context.Create(assembly, FieldSource.Properties);

            var tree = context.MapTree($"{typeof(TreeMapperTests).FullName}+IRootInterface");

            var expectedTree = TreeDefinitionBuilder.Create($"{typeof(TreeMapperTests).FullName}+IRootInterface", b =>
            {
                var rootAlias = b.PushAlias(
                    $"{typeof(TreeMapperTests).FullName}+IRootInterface",
                    $"{typeof(TreeMapperTests).FullName}+TestNodeB");

                var innerAlias = b.PushAlias(
                    $"{typeof(TreeMapperTests).FullName}+IInnerInterface",
                    $"{typeof(TreeMapperTests).FullName}+TestNodeA",
                    $"{typeof(TreeMapperTests).FullName}+TestNodeB");

                b.PushNode($"{typeof(TreeMapperTests).FullName}+TestNodeB", nb =>
                {
                    nb.PushNumberField("Field1");
                    nb.PushAliasField("Field2", innerAlias);
                });
                b.PushNode($"{typeof(TreeMapperTests).FullName}+TestNodeA", nb =>
                {
                    nb.PushStringField("Field1");
                    nb.PushNumberField("Field2");
                    nb.PushBooleanField("Field3");
                    nb.PushAliasField("Field4", innerAlias, isArray: true);
                    nb.PushAliasField("Field5", innerAlias);
                });
            });

            Assert.Equal(expectedTree, tree);
        }

        private class TestNodeA : IInnerInterface
        {
            public string Field1 { get; set; }

            public float Field2 { get; set; }

            public bool Field3 { get; set; }

            public IReadOnlyList<IInnerInterface> Field4 { get; set; }

            public IInnerInterface Field5 { get; set; }
        }

        private class TestNodeB : IRootInterface, IInnerInterface
        {
            public float Field1 { get; set; }

            public IInnerInterface Field2 { get; set; }
        }
    }
}
