using System.Collections.Generic;
using System.Text.RegularExpressions;
using CommandLine;
using CommandLine.Text;

using TypedTree.Generator.Core.Mapping;

namespace TypedTree.Generator.Cli
{
    /// <summary>
    /// Class containing all options that can be passed to the cli tool.
    /// </summary>
    public sealed class ProgramOptions
    {
        /// <summary>List of examples used for generating the helptext</summary>
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

        /// <summary>Path to assembly to generate the scheme for</summary>
        [Option('a', "assembly", Required = true, HelpText = "Assembly file to generate the scheme from")]
        public string AssemblyFile { get; set; }

        /// <summary>Optional list of directories to search for dependencies</summary>
        [Option('d', "dependencies", Required = false, HelpText = "Directories to look for dependencies")]
        public IEnumerable<string> DependencyDirectories { get; set; }

        /// <summary>Name of root-type of the scheme</summary>
        [Option('r', "root-type", Required = true, HelpText = "FullName of type to use as the root of the tree")]
        public string RootType { get; set; }

        /// <summary>Path to generate output scheme to</summary>
        [Option('o', "output", Required = true, HelpText = "Path where to save the generated scheme")]
        public string OutputPath { get; set; }

        /// <summary>Source to look for fields on types</summary>
        [Option('s', "field-source", Required = false, Default = FieldSource.PublicProperties, HelpText = "Where to look for fields on nodes")]
        public FieldSource FieldSource { get; set; }

        /// <summary>Optional regex pattern to ignore types</summary>
        [Option('i', "ignore", Required = false, HelpText = "Regex pattern used to ignore types")]
        public Regex TypeIgnorePattern { get; set; }

        /// <summary>Switch to enable verbose logging</summary>
        [Option('v', "verbose", Required = false, HelpText = "Enable verbose output")]
        public bool Verbose { get; set; }
    }
}
