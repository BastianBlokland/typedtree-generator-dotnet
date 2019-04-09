using System;
using Microsoft.Extensions.Logging;

namespace TypedTree.Generator.Cli
{
    public sealed class ConsoleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string name) => new ConsoleLogger(name);

        public void Dispose()
        {
        }
    }

    public sealed class ConsoleLogger : ILogger
    {
        private readonly object lockObject = new object();
        private readonly string name;

        public ConsoleLogger(string name) => this.name = name;

        public IDisposable BeginScope<T>(T state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<T>(
            LogLevel level,
            EventId id,
            T state,
            Exception exception,
            Func<T, Exception, string> formatter)
        {
            lock (this.lockObject)
            {
                // Set color
                var prevColor = Console.ForegroundColor;
                Console.ForegroundColor = GetColor();

                // Write
                Console.WriteLine($"{GetFormattedLevel()}: {formatter(state, exception)}");

                // Restore color
                Console.ForegroundColor = prevColor;
            }

            string GetFormattedLevel()
            {
                switch (level)
                {
                    case LogLevel.Trace: return "trace";
                    case LogLevel.Debug: return "debug";
                    case LogLevel.Information: return "info";
                    case LogLevel.Warning: return "warn";
                    case LogLevel.Error: return "fail";
                    case LogLevel.Critical: return "crit";
                    default:
                        throw new ArgumentException($"Unknown level: '{level}'", nameof(level));
                }
            }

            ConsoleColor GetColor()
            {
                switch (level)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug: return ConsoleColor.DarkGreen;
                    case LogLevel.Information: return ConsoleColor.White;
                    case LogLevel.Warning: return ConsoleColor.DarkYellow;
                    case LogLevel.Error:
                    case LogLevel.Critical: return ConsoleColor.Red;
                    default:
                        throw new ArgumentException($"Unknown level: '{level}'", nameof(level));
                }
            }
        }
    }
}
