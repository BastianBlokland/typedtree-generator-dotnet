using System;
using TypedTreeGenerator.Core;

namespace TypedTreeGenerator.Cli
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
