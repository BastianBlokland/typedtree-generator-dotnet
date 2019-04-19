using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            // Create load context.
            var loadContext = CreateLoadContext(assemblyPath, logger);
            if (loadContext == null)
                return null;

            // Load assemblies.
            var assemblies = LoadAssemblyFromPath(loadContext, assemblyPath, logger);
            logger?.LogDebug($"Loaded {assemblies.Count} assemblies");

            // Load types.
            var typeCollection = TypeCollection.Create(assemblies, logger);
            logger?.LogDebug($"Loaded {typeCollection.TypeCount} types");
            return typeCollection;
        }

        private static IReadOnlyList<Assembly> LoadAssemblyFromPath(
            AssemblyLoadContext loadContext,
            string path,
            ILogger logger = null)
        {
            // Validate path.
            var validatedPath = ValidateFilePath(path, logger);
            if (validatedPath == null)
                return Array.Empty<Assembly>();

            // Load assembly.
            var output = new List<Assembly>();
            try
            {
                output.Add(loadContext.LoadFromAssemblyPath(validatedPath));
                logger?.LogTrace($"Loaded assembly: '{output[0].FullName}'");
            }
            catch (Exception e)
            {
                logger?.LogCritical($"Failed to load assembly: {e.Message}");
                return Array.Empty<Assembly>();
            }

            // Load referenced assemblies.
            foreach (var referencedAssemblyName in output[0].GetReferencedAssemblies())
            {
                if (!IsAssemblyLoaded(referencedAssemblyName))
                    output.AddRange(LoadAssemblyFromName(loadContext, referencedAssemblyName, logger));
            }

            return output;
        }

        private static IReadOnlyList<Assembly> LoadAssemblyFromName(
            AssemblyLoadContext loadContext,
            AssemblyName assemblyName,
            ILogger logger = null)
        {
            // Load assembly.
            var output = new List<Assembly>();
            try
            {
                output.Add(loadContext.LoadFromAssemblyName(assemblyName));
                logger?.LogTrace($"Loaded assembly: '{output[0].FullName}'");
            }
            catch (Exception e)
            {
                logger?.LogCritical($"Failed to load assembly: {e.Message}");
                return Array.Empty<Assembly>();
            }

            // Load referenced assemblies.
            foreach (var referencedAssemblyName in output[0].GetReferencedAssemblies())
            {
                if (!IsAssemblyLoaded(referencedAssemblyName))
                    output.AddRange(LoadAssemblyFromName(loadContext, referencedAssemblyName, logger));
            }

            return output;
        }

        private static bool IsAssemblyLoaded(AssemblyName assemblyName) =>
            AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().FullName == assemblyName.FullName);

        private static AssemblyLoadContext CreateLoadContext(string assemblyPath, ILogger logger = null)
        {
            var validatedPath = ValidateFilePath(assemblyPath, logger);
            if (validatedPath == null)
                return null;

            var mainAssemblyName = Path.GetFileNameWithoutExtension(validatedPath);
            var builder = new AssemblyLoadContextBuilder();

            // Set base directory.
            var baseDir = Path.GetDirectoryName(validatedPath);
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

        private static string ValidateFilePath(string path, ILogger logger = null)
        {
            // Determine full-path.
            string fullPath;
            try
            {
                fullPath = Path.GetFullPath(path);
            }
            catch
            {
                logger?.LogCritical($"Unable to determine absolute path for: '{path}'");
                return null;
            }

            // Validate file existence.
            if (!File.Exists(fullPath))
            {
                logger?.LogCritical($"No file found at path: '{fullPath}'");
                return null;
            }

            return fullPath;
        }
    }
}
