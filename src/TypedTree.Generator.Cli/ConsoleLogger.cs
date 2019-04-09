using System;
using Microsoft.Extensions.Logging;

namespace TypedTree.Generator.Cli
{
    public sealed class ConsoleLoggerProvider : ILoggerProvider
    {
        public ConsoleLoggerProvider(bool verbose) => this.Verbose = verbose;

        public bool Verbose { get; set; }

        public ILogger CreateLogger(string name) => new ConsoleLogger(name, this.Verbose);

        public void Dispose()
        {
        }
    }

    public sealed class ConsoleLogger : ILogger
    {
        private readonly object lockObject = new object();
        private readonly string name;
        private readonly bool verbose;

        public ConsoleLogger(string name, bool verbose)
        {
            this.name = name;
            this.verbose = verbose;
        }

        public IDisposable BeginScope<T>(T state) => null;

        public bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug: return this.verbose;
                case LogLevel.Information:
                case LogLevel.Warning:
                case LogLevel.Error:
                case LogLevel.Critical: return true;
                default:
                    throw new ArgumentException($"Unknown level: '{level}'", nameof(level));
            }
        }

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
                    case LogLevel.Trace: return ConsoleColor.DarkGray;
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
