using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

using TypedTree.Generator.Core.Mapping;
using TypedTree.Generator.Core.Mapping.NodeComments;
using TypedTree.Generator.Core.Scheme;
using TypedTree.Generator.Core.Serialization;

namespace TypedTree.Generator.Cli
{
    /// <summary>
    /// Application logic for the cli tool.
    /// </summary>
    public sealed class Application
    {
        private readonly LoggerAdapter logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="logger">Optional logger to use during execution</param>
        public Application(ILogger<Application> logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            this.logger = new LoggerAdapter(logger);
        }

        /// <summary>
        /// Run the generator tool.
        /// </summary>
        /// <param name="assemblyFile">Assembly to generate the scheme for</param>
        /// <param name="dependencyDirectories">Optional paths to look for dependencies</param>
        /// <param name="rootType">Root type of the tree</param>
        /// <param name="fieldSource">Source to find fields</param>
        /// <param name="typeIgnorePattern">Optional regex pattern for ignoring types</param>
        /// <param name="outputPath">Path where to generate the output scheme to</param>
        /// <returns>Exit code</returns>
        public int Run(
            string assemblyFile,
            IEnumerable<string> dependencyDirectories,
            string rootType,
            FieldSource fieldSource,
            Regex typeIgnorePattern,
            string outputPath)
        {
            if (assemblyFile == null)
                throw new ArgumentNullException(nameof(assemblyFile));
            if (dependencyDirectories == null)
                throw new ArgumentNullException(nameof(dependencyDirectories));
            if (rootType == null)
                throw new ArgumentNullException(nameof(rootType));

            // Load types from given assembly.
            var typeCollection = TypeLoader.TryLoad(assemblyFile, dependencyDirectories, this.logger);
            if (typeCollection == null)
                return 1;

            // Verify that root-type can be found in those types.
            if (!typeCollection.TryGetType(rootType, out _))
            {
                this.logger.LogCritical($"Unable to find root-type: '{rootType}'");
                return 1;
            }

            // Load doc-comment file for providing node comments.
            XmlNodeCommentProvider nodeCommentProvider = null;
            var xmlDocFilePath = Path.ChangeExtension(assemblyFile, "xml");
            if (File.Exists(xmlDocFilePath))
            {
                this.logger.LogInformation($"Using doc-comment file: '{xmlDocFilePath}'");
                using (var stream = new FileStream(xmlDocFilePath, FileMode.Open, FileAccess.Read))
                {
                    if (!XmlNodeCommentProvider.TryParse(stream, out nodeCommentProvider))
                        this.logger?.LogWarning($"Failed to parse doc-comment file: '{xmlDocFilePath}'");
                }
            }

            // Create context object containing all the settings for the mapping.
            var context = Context.Create(
                typeCollection,
                fieldSource,
                typeIgnorePattern,
                nodeCommentProvider,
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
    }
}
