using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

using TypedTree.Generator.Core.Mapping;

namespace TypedTree.Generator.Cli
{
    public sealed class Application
    {
        private readonly ILogger logger;

        public Application(ILogger<Application> logger)
        {
            this.logger = logger;
        }

        public int Run(
            string assemblyFile,
            string rootType,
            FieldSource fieldSource,
            Regex typeIgnorePattern,
            string outputPath)
        {
            this.logger.LogInformation("running..");
            this.logger.LogDebug($"assemblyFile: {assemblyFile}");
            this.logger.LogDebug($"rootType: {rootType}");
            this.logger.LogDebug($"fieldSource: {fieldSource}");
            this.logger.LogDebug($"typeIgnorePattern: {typeIgnorePattern}");
            this.logger.LogDebug($"outputPath: {outputPath}");
            return 0;
        }
    }
}
