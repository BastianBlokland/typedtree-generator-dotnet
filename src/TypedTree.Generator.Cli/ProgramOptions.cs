using System.Collections.Generic;
using System.Text.RegularExpressions;
using CommandLine;
using CommandLine.Text;

using TypedTree.Generator.Core.Mapping;

namespace TypedTree.Generator.Cli
{
    public sealed class ProgramOptions
    {
        [Usage(ApplicationAlias = "typedtree-generator")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Generate a tree-schene", new ProgramOptions
                {
                    AssemblyFile = "Path/To/Example.dll",
                    RootType = "Game.AI.IBehaviourNode",
                    OutputPath = "Path/To/ai.treescheme.json"
                });
            }
        }

        [Option('a', "assembly", Required = true, HelpText = "Assembly file to generate the scheme from")]
        public string AssemblyFile { get; set; }

        [Option('d', "dependencies", Required = false, HelpText = "Directories to look for dependencies")]
        public IEnumerable<string> DependencyDirectories { get; set; }

        [Option('r', "root-type", Required = true, HelpText = "FullName of type to use as the root of the tree")]
        public string RootType { get; set; }

        [Option('o', "output", Required = true, HelpText = "Path where to save the generated scheme")]
        public string OutputPath { get; set; }

        [Option('s', "field-source", Required = false, Default = FieldSource.PublicProperties, HelpText = "Where to look for fields on nodes")]
        public FieldSource FieldSource { get; set; }

        [Option('i', "ignore", Required = false, HelpText = "Regex pattern used to ignore types")]
        public Regex TypeIgnorePattern { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Enable verbose output")]
        public bool Verbose { get; set; }
    }
}
