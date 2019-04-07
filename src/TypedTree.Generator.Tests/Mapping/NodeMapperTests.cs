#pragma warning disable CA1812 // Avoid uninstantiated internal classes

using System.Collections.Generic;
using System.Reflection;
using Xunit;

using TypedTree.Generator.Core.Mapping;
using TypedTree.Generator.Core.Builder;

namespace TypedTree.Generator.Tests.Mapping
{
    public sealed class NodeMapperTests
    {
        private enum TestEnum
        {
            A,
            B
        }

        private interface ITestInterface
        {
        }

        [Fact]
        public void NodeIsMappedAccordingToSchemeRules()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var context = Context.Create(assembly, FieldSource.Properties);

            var tree = TreeDefinitionBuilder.Create($"{typeof(NodeMapperTests).FullName}+TestNodeA", b =>
            {
                b.MapNode(context, typeof(TestNodeA));
            });
            var expectedTree = TreeDefinitionBuilder.Create($"{typeof(NodeMapperTests).FullName}+TestNodeA", b =>
            {
                var nodeA = b.PushAlias(
                    $"{typeof(NodeMapperTests).FullName}+TestNodeA",
                    $"{typeof(NodeMapperTests).FullName}+TestNodeA");

                var nodeB = b.PushAlias(
                    $"{typeof(NodeMapperTests).FullName}+TestNodeB",
                    $"{typeof(NodeMapperTests).FullName}+TestNodeB");

                var nodeAorB = b.PushAlias(
                    $"{typeof(NodeMapperTests).FullName}+ITestInterface",
                    $"{typeof(NodeMapperTests).FullName}+TestNodeA",
                    $"{typeof(NodeMapperTests).FullName}+TestNodeB");

                var @enum = b.PushEnum(
                    $"{typeof(NodeMapperTests).FullName}+TestEnum",
                    ("A", 0),
                    ("B", 1));

                b.PushNode($"{typeof(NodeMapperTests).FullName}+TestNodeA", nb =>
                {
                    nb.PushStringField("Field1");
                    nb.PushNumberField("Field2");
                    nb.PushBooleanField("Field3");
                    nb.PushEnumField("Field4", @enum);
                    nb.PushAliasField("Field5", nodeA, isArray: true);
                    nb.PushAliasField("Field6", nodeB);
                });
                b.PushNode($"{typeof(NodeMapperTests).FullName}+TestNodeB", nb =>
                {
                    nb.PushNumberField("Field1");
                    nb.PushAliasField("Field2", nodeAorB);
                });
            });

            Assert.Equal(expectedTree, tree);
        }

        [Fact]
        public void NodeCanBeMappedMultipleTimes()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var context = Context.Create(assembly, FieldSource.Properties);

            TreeDefinitionBuilder.Create($"{typeof(NodeMapperTests).FullName}+TestNodeA", b =>
            {
                var nodeA1 = b.MapNode(context, typeof(TestNodeA));
                var nodeA2 = b.MapNode(context, typeof(TestNodeA));

                Assert.Equal(nodeA1, nodeA2);
            });
        }

        private class TestNodeA : ITestInterface
        {
            public string Field1 { get; set; }

            public float Field2 { get; set; }

            public bool Field3 { get; set; }

            public TestEnum Field4 { get; set; }

            public IReadOnlyList<TestNodeA> Field5 { get; set; }

            public TestNodeB Field6 { get; set; }

            public object InvalidField { get; set; }
        }

        private class TestNodeB : ITestInterface
        {
            public float Field1 { get; set; }

            public ITestInterface Field2 { get; set; }
        }
    }
}
