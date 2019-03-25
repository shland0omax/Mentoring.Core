using Microsoft.Extensions.Logging;

namespace Mentoring.Core.Module1.Services
{
    public class FileLoggerProvider: ILoggerProvider
    {
        private readonly string _path;

        public FileLoggerProvider(string path)
        {
            _path = path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path, LogLevel.Debug);
        }

        public void Dispose()
        {
        }
    }
}
