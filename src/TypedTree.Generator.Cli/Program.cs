using System;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TypedTree.Generator.Cli
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            // Parse Options from args and call 'MainWithOptions' method if succeeded, otherwise show help
            var parser = new Parser(settings =>
            {
                settings.CaseInsensitiveEnumValues = true;
            });
            var parseResult = parser.ParseArguments<ProgramOptions>(args);
            return parseResult.MapResult(
                options => MainWithOptions(options),
                errors =>
                {
                    // Show help
                    var help = HelpText.AutoBuild(parseResult);
                    help.AddEnumValuesToHelpText = true;
                    help.Copyright = "TypedTree Generator - MIT Licence";
                    help.MaximumDisplayWidth = 100;
                    help.AddOptions(parseResult);
                    Console.Error.Write(help);

                    // Exit with error
                    return 1;
                });
        }

        public static int MainWithOptions(ProgramOptions options)
        {
            // Configure services
            var services = new ServiceCollection();
            ConfigureServices(services, options.Verbose);

            // Run application
            using (var provider = services.BuildServiceProvider())
            {
                return provider.GetService<Application>().Run(
                    options.AssemblyFile,
                    options.RootType,
                    options.FieldSource,
                    options.TypeIgnorePattern,
                    options.OutputPath);
            }
        }

        private static void ConfigureServices(IServiceCollection services, bool verboseLogging)
        {
            // Logging
            services.AddLogging(logConfig =>
            {
                logConfig.AddProvider(new ConsoleLoggerProvider()).
                    SetMinimumLevel(verboseLogging ? LogLevel.Debug : LogLevel.Information);
            });

            // Application
            services.AddTransient<Application>();
        }
    }
}
