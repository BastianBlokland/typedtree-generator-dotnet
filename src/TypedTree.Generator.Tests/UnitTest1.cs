using System;
using TypedTree.Generator.Core;
using Xunit;

namespace TypedTree.Generator.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("Hello World", Class1.HelloWorld());
        }
    }
}