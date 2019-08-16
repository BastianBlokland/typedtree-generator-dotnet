using System;
using Microsoft.Extensions.Logging;

namespace TypedTree.Generator.Cli
{
    /// <summary>
    /// Adapt a <see cref="Microsoft.Extensions.Logging.ILogger"/> to a <see cref="Core.ILogger"/>.
    /// </summary>
    public sealed class LoggerAdapter : Core.ILogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger sink;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerAdapter"/> class.
        /// </summary>
        /// <param name="sink">Logger to output to.</param>
        public LoggerAdapter(Microsoft.Extensions.Logging.ILogger sink)
        {
            this.sink = sink ?? throw new ArgumentNullException(nameof(sink));
        }

        /// <summary>
        /// Check if a certain log-level of the underlying sink enabled.
        /// </summary>
        /// <param name="level">Level to check for.</param>
        /// <returns>True if enabled otherwise False.</returns>
        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel level) =>
            this.sink.IsEnabled(level);

        /// <inheritdoc/>
        public void LogTrace(string message) => this.sink.LogTrace(message);

        /// <inheritdoc/>
        public void LogDebug(string message) => this.sink.LogDebug(message);

        /// <inheritdoc/>
        public void LogInformation(string message) => this.sink.LogInformation(message);

        /// <inheritdoc/>
        public void LogWarning(string message) => this.sink.LogWarning(message);

        /// <inheritdoc/>
        public void LogError(string message) => this.sink.LogError(message);

        /// <inheritdoc/>
        public void LogCritical(string message) => this.sink.LogCritical(message);
    }
}
