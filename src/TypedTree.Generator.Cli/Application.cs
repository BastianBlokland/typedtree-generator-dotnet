using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Logging;

using TypedTree.Generator.Core.Mapping;
using TypedTree.Generator.Core.Scheme;
using TypedTree.Generator.Core.Utilities;
using TypedTree.Generator.Core.Serialization;

namespace TypedTree.Generator.Cli
{
    public sealed class Application
    {
        private readonly ILogger logger;

        public Application(ILogger<Application> logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this.logger = logger;
        }

        public int Run(
            string assemblyFile,
            string rootType,
            FieldSource fieldSource,
            Regex typeIgnorePattern,
            string outputPath)
        {
            // Load all the assemblies (the provided assembly and the assemblies that are referenced).
            var assemblies = this.LoadAssemblies(assemblyFile).ToArray();
            if (assemblies == null || assemblies.Length == 0)
                return 1;
            this.logger.LogDebug($"Finished loading '{assemblies.Length}' assemblies");

            // Load all the types from those assemblies.
            var typeCollection = TypeCollection.Create(assemblies, this.logger);
            this.logger.LogDebug($"Finished loading '{typeCollection.TypeCount}' types");

            // Verify that root-type can be found in those types.
            if (!typeCollection.TryGetType(rootType, out _))
            {
                this.logger.LogCritical($"Unable to find root-type: '{rootType}'");
                return 1;
            }

            // Create context object containing all the settings for the mapping.
            var context = Context.Create(
                typeCollection,
                fieldSource,
                typeIgnorePattern,
                this.logger.IsEnabled(LogLevel.Debug) ? this.logger : null);

            // Map the tree.
            TreeDefinition tree = null;
            try
            {
                tree = TreeMapper.MapTree(context, rootType);

                this.logger.LogInformation(
                    $"Mapped tree (aliases: '{tree.Aliases.Length}', enums: '{tree.Enums.Length}', nodes: '{tree.Nodes.Length}')");
            }
            catch (Core.Mapping.Exceptions.MappingFailureException e)
            {
                this.logger.LogCritical($"Failed to map: '{e.InnerException.Message}'");
                return 1;
            }

            // Save the result.
            try
            {
                Directory.GetParent(outputPath).Create();
                using (var stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    JsonSerializer.WriteJson(tree, JsonSerializer.Mode.Pretty, stream);
                }

                this.logger.LogInformation($"Written scheme to: '{outputPath}'");
            }
            catch (Exception e)
            {
                this.logger.LogCritical($"Failed to save to '{outputPath}': {e.Message}");
                return 1;
            }

            return 0;
        }

        private IEnumerable<Assembly> LoadAssemblies(string assemblyFile)
        {
            if (!File.Exists(assemblyFile))
            {
                this.logger.LogCritical($"No file found at path: '{assemblyFile}'");
                yield break;
            }

            PluginLoader loader = null;
            Assembly mainAssembly = null;
            try
            {
                loader = PluginLoader.CreateFromAssemblyFile(assemblyFile);
                mainAssembly = loader.LoadDefaultAssembly();
                this.logger.LogDebug($"Loaded main-assembly: '{mainAssembly.FullName}'");
            }
            catch
            {
                this.logger.LogCritical($"Failed to load assembly: '{assemblyFile}'");
                yield break;
            }

            yield return mainAssembly;

            foreach (var refAssemblyName in mainAssembly.GetReferencedAssemblies())
            {
                if (IsSystemAssembly(refAssemblyName))
                {
                    this.logger.LogDebug($"Skipping sys-assembly: '{refAssemblyName}'");
                    continue;
                }

                Assembly refAssembly = null;
                try
                {
                    refAssembly = loader.LoadAssembly(refAssemblyName);
                    this.logger.LogDebug($"Loaded ref-assembly: '{refAssembly.FullName}'");
                }
                catch
                {
                    this.logger.LogCritical($"Failed to load ref-assembly: '{refAssemblyName}'");
                }

                if (refAssembly != null)
                    yield return refAssembly;
            }

            bool IsSystemAssembly(AssemblyName assemblyName) =>
                assemblyName.FullName.StartsWith("System", StringComparison.Ordinal) ||
                assemblyName.FullName.StartsWith("Microsoft", StringComparison.Ordinal);
        }
    }
}
