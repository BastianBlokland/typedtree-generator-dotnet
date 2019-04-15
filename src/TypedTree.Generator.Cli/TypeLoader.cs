using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using McMaster.NETCore.Plugins.Loader;
using Microsoft.Extensions.Logging;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Cli
{
    public static class TypeLoader
    {
        public static ITypeCollection TryLoad(string assemblyPath, ILogger logger = null)
        {
            // Determine full-path.
            string assemblyFilePath;
            try
            {
                assemblyFilePath = Path.GetFullPath(assemblyPath);
            }
            catch
            {
                logger?.LogCritical($"Unable to determine absolute path for: '{assemblyPath}'");
                return null;
            }

            // Validate file existence.
            if (!File.Exists(assemblyFilePath))
            {
                logger?.LogCritical($"No file found at path: '{assemblyFilePath}'");
                return null;
            }

            // Create load context.
            var loadContext = CreateLoadContext(assemblyFilePath, logger);

            // Load assemblies.
            var assemblies = LoadAssemblies();
            logger?.LogDebug($"Loaded {assemblies.Count} assemblies");

            var typeCollection = TypeCollection.Create(assemblies, logger);
            logger?.LogDebug($"Loaded {typeCollection.TypeCount} types");
            return typeCollection;

            IReadOnlyList<Assembly> LoadAssemblies()
            {
                Assembly mainAssembly = null;
                try
                {
                    mainAssembly = loadContext.LoadFromAssemblyPath(assemblyFilePath);
                    logger?.LogTrace($"Loaded main-assembly: '{mainAssembly.FullName}'");
                }
                catch (Exception e)
                {
                    logger?.LogCritical($"Failed to load main-assembly: {e.Message}");
                    return null;
                }

                var result = new List<Assembly> { mainAssembly };
                foreach (var referencedAssemblyName in mainAssembly.GetReferencedAssemblies())
                {
                    Assembly refAssembly = null;
                    try
                    {
                        refAssembly = loadContext.LoadFromAssemblyName(referencedAssemblyName);
                        logger?.LogTrace($"Loaded ref-assembly: '{refAssembly.FullName}'");
                    }
                    catch (Exception e)
                    {
                        logger?.LogCritical(
                            $"Failed to load referenced-assembly '{referencedAssemblyName}': {e.Message}");
                    }

                    if (refAssembly != null)
                        result.Add(refAssembly);
                }

                return result;
            }
        }

        private static AssemblyLoadContext CreateLoadContext(string assemblyFilePath, ILogger logger = null)
        {
            var mainAssemblyName = Path.GetFileNameWithoutExtension(assemblyFilePath);
            var builder = new AssemblyLoadContextBuilder();

            // Set base directory.
            var baseDir = Path.GetDirectoryName(assemblyFilePath);
            builder.SetBaseDirectory(baseDir);
            logger?.LogTrace($"Base directory: '{baseDir}'");

            // Add deps file as a source for finding dependencies.
            var depsJsonFile = Path.Combine(baseDir, $"{mainAssemblyName}.deps.json");
            if (File.Exists(depsJsonFile))
            {
                builder.AddDependencyContext(depsJsonFile);
                logger?.LogTrace($"Added '{depsJsonFile}' as a deps file dependency");
            }

            // Add runtimeconfig file as a source for finding dependencies.
            var pluginRuntimeConfigFile = Path.Combine(baseDir, $"{mainAssemblyName}.runtimeconfig.json");
            builder.TryAddAdditionalProbingPathFromRuntimeConfig(pluginRuntimeConfigFile, includeDevConfig: true, out _);

            return builder.Build();
        }
    }
}
