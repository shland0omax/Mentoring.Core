using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Mentoring.Core.Module1.Services
{
    public class FileLogger: ILogger
    {
        private readonly string _filePath;
        private readonly LogLevel _minLogLevel;
        private static readonly object _lock = new object();

        public FileLogger(string path, LogLevel logLevel = LogLevel.Information)
        {
            _filePath = path;
            _minLogLevel = logLevel;
        }

        public FileLogger(IConfiguration configuration, string setting, LogLevel logLevel = LogLevel.Information)
        {
            _filePath = configuration.GetValue<string>(setting);
            _minLogLevel = logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minLogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null) return;
            lock (_lock)
            {
                var log = $"{logLevel}: {formatter(state, exception) + Environment.NewLine}";
                File.AppendAllText(_filePath, log);
            }
        }
    }
}
