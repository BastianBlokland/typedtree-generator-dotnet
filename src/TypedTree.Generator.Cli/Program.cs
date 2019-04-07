using System;
using CommandLine;
using CommandLine.Text;

namespace TypedTree.Generator.Cli
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            // Parse Options from args and call 'GenerateScheme' if succeeded, otherwise show help
            var parser = new Parser(settings =>
            {
                settings.CaseInsensitiveEnumValues = true;
            });
            var parseResult = parser.ParseArguments<Options>(args);
            return parseResult.MapResult(
                options => GenerateScheme(options),
                errors =>
                {
                    // Show help
                    var help = HelpText.AutoBuild(parseResult);
                    help.AddEnumValuesToHelpText = true;
                    help.Copyright = "TypedTree - MIT Licence";
                    help.MaximumDisplayWidth = 100;
                    help.AddOptions(parseResult);
                    Console.Error.Write(help);

                    // Exit with error
                    return 1;
                });
        }

        private static int GenerateScheme(Options options)
        {
            Console.WriteLine($"AssemblyFile: {options.AssemblyFile}");
            Console.WriteLine($"FieldSource: {options.FieldSource}");
            Console.WriteLine($"OutputPath: {options.OutputPath}");
            Console.WriteLine($"RootType: {options.RootType}");
            Console.WriteLine($"TypeIgnorePattern: {options.TypeIgnorePattern}");
            Console.WriteLine($"Verbose: {options.Verbose}");
            return 0;
        }
    }
}
