using Mentoring.Core.Module1.Services;
using Microsoft.Extensions.Logging;

namespace Mentoring.Core.Module1.Middleware
{
    public static class LoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string path)
        {
            factory.AddProvider(new FileLoggerProvider(path));
            return factory;
        }
    }
}