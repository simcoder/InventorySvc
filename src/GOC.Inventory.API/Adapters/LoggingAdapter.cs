using System;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.API.Adapters
{
    public class LoggingAdapter<T> : ILogger<T>
    {
        private readonly ILogger _adaptee;

        public LoggingAdapter(ILoggerFactory factory)
        {
            _adaptee = factory.CreateLogger<T>();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _adaptee.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _adaptee.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            _adaptee.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
