using System;
using TypedTree.Generator.Core;

namespace TypedTree.Generator.Cli
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"NumArgs: {args.Length}");
            Console.WriteLine(Class1.HelloWorld());
        }
    }
}
