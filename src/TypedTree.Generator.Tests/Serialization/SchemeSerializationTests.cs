using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Xunit;

using TypedTree.Generator.Core.Scheme;
using TypedTree.Generator.Core.Builder;
using TypedTree.Generator.Core.Serialization;

namespace TypedTree.Generator.Tests.Serialization
{
    public sealed class SchemeSerializationTests
    {
        public static IEnumerable<object[]> GetTestTreeJsonPairsData()
        {
            foreach (var tuple in GetTestTreeJsonPairs())
                yield return new object[] { tuple.tree, tuple.json };
        }

        [Theory]
        [MemberData(nameof(GetTestTreeJsonPairsData))]
        public void CorrectJsonOutputIsGenerated(TreeDefinition tree, string expectedJson)
        {
            // Create json for the tree
            var writtenJson = tree.ToJson(JsonSerializer.Mode.Normal);

            // Strip whitespace (Easier to define test-data without having to worry about whitespace)
            writtenJson = writtenJson.Replace(" ", string.Empty, StringComparison.Ordinal);
            expectedJson = expectedJson.Replace(" ", string.Empty, StringComparison.Ordinal);
            expectedJson = expectedJson.Replace("\n", string.Empty, StringComparison.Ordinal);

            // Assert that written and expected json match.
            Assert.Equal(expectedJson, writtenJson);
        }

        [Fact]
        public void StringAndStreamMethodsProduceTheSameJson()
        {
            var scheme = GetTestTreeJsonPairs().First().tree;
            using (var stream = new MemoryStream())
            {
                // Create json with string method
                var stringText = scheme.ToJson(JsonSerializer.Mode.Pretty);
                var stringBytes = Encoding.UTF8.GetBytes(stringText);

                // Create json with stream method
                scheme.WriteJson(JsonSerializer.Mode.Pretty, stream);
                var streamBytes = stream.ToArray();

                // Verify that they are equal
                Assert.True(stringBytes.SequenceEqual(streamBytes));
            }
        }

        private static IEnumerable<(TreeDefinition tree, string json)> GetTestTreeJsonPairs()
        {
            yield return
            (
                tree: TreeDefinitionBuilder.Create("AliasA", b =>
                {
                    b.PushAlias("AliasA", "NodeA");
                    b.PushNode("NodeA");
                }),
                json: @"{
                    ""rootAlias"": ""AliasA"",
                    ""aliases"": [
                        { ""identifier"": ""AliasA"", ""values"": [ ""NodeA"" ] }
                    ],
                    ""enums"": [ ],
                    ""nodes"": [
                        { ""nodeType"": ""NodeA"", ""fields"": [] }
                    ]
                }"
            );

            yield return
            (
                tree: TreeDefinitionBuilder.Create("Alias", b =>
                {
                    var alias = b.PushAlias("Alias", "NodeA", "NodeB");
                    var @enum = b.PushEnum("Enum", ("A", 0), ("B", 1));
                    b.PushNode("NodeA");
                    b.PushNode("NodeB", bn =>
                    {
                        bn.PushBooleanField("field1");
                        bn.PushStringField("field2");
                        bn.PushNumberField("field3", isArray: true);
                        bn.PushAliasField("field4", alias, isArray: true);
                        bn.PushEnumField("field5", @enum, isArray: true);
                    });
                }),
                json: @"{
                    ""rootAlias"": ""Alias"",
                    ""aliases"": [
                        { ""identifier"": ""Alias"", ""values"": [ ""NodeA"", ""NodeB"" ] }
                    ],
                    ""enums"": [
                        { ""identifier"": ""Enum"", ""values"": [
                            { ""value"": 0, ""name"": ""A"" }, { ""value"": 1, ""name"": ""B"" }
                        ]}
                    ],
                    ""nodes"": [
                        {
                            ""nodeType"": ""NodeA"",
                            ""fields"": []
                        },
                        {
                            ""nodeType"": ""NodeB"",
                            ""fields"": [
                                { ""name"": ""field1"", ""valueType"": ""boolean"" },
                                { ""name"": ""field2"", ""valueType"": ""string"" },
                                { ""name"": ""field3"", ""valueType"": ""number"", ""isArray"": true },
                                { ""name"": ""field4"", ""valueType"": ""Alias"", ""isArray"": true },
                                { ""name"": ""field5"", ""valueType"": ""Enum"", ""isArray"": true }
                            ]
                        }
                    ]
                }"
            );
        }
    }
}
