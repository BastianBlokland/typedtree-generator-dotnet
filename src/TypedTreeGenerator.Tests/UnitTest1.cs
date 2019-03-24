using System;
using TypedTreeGenerator.Core;
using Xunit;

namespace TypedTreeGenerator.Tests
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
